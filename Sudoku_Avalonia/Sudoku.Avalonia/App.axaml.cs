using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using ELTE.Sudoku.Model;
using ELTE.Sudoku.Persistence;
using ELTE.Sudoku.Avalonia.ViewModels;
using ELTE.Sudoku.Avalonia.Views;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using System.ComponentModel;
using System.IO;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Avalonia.Platform;
using Avalonia.Threading;

namespace ELTE.Sudoku.Avalonia;

public partial class App : Application
{
    #region Fields

    private LabyrinthGameModel _model = null!;
    private LabyrinthViewModel _viewModel = null!;

    #endregion

    #region Properites

    private TopLevel? TopLevel
    {
        get
        {
            return ApplicationLifetime switch
            {
                IClassicDesktopStyleApplicationLifetime desktop => TopLevel.GetTopLevel(desktop.MainWindow),
                ISingleViewApplicationLifetime singleViewPlatform => TopLevel.GetTopLevel(singleViewPlatform.MainView),
                _ => null
            };
        }
    }

    #endregion

    #region Application methods
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // Line below is needed to remove Avalonia data validation.
        // Without this line you will get duplicate validations from both Avalonia and CT
        BindingPlugins.DataValidators.RemoveAt(0);

        // modell létrehozása
        _model = new LabyrinthGameModel(new LabyrinthFileDataAccess(), new LabyrinthTimerInheritance());
        _model.GameOver += new EventHandler<LabyrinthEventArgs>(Model_GameOver);
        //_model.NewGame();

        // nézemodell létrehozása
        _viewModel = new LabyrinthViewModel(_model);
        _viewModel.NewGame += new EventHandler(ViewModel_NewGame);
        _viewModel.ExitGame += new EventHandler(ViewModel_ExitGame);
        _viewModel.PauseGame += new EventHandler(ViewModel_PauseGame);
        //_viewModel.LoadGame += new EventHandler(ViewModel_LoadGame);
        //_viewModel.SaveGame += new EventHandler(ViewModel_SaveGame);
        _model.FirsPosition();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = _viewModel
            };

            desktop.Startup += (s, e) =>
            {
                _model.NewGame(); // indításkor új játékot kezdünk
                _model.FirsPosition();
            };



        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView
            {
                DataContext = _viewModel
            };

            
                _model.NewGame(); // indításkor új játékot kezdünk
                _model.FirsPosition();
            
            
            if (Application.Current?.TryGetFeature<IActivatableLifetime>() is { } activatableLifetime)
            {
                activatableLifetime.Activated +=  (sender, args) =>
                {
                    if (args.Kind == ActivationKind.Background)
                    {
                        // betöltjük a felfüggesztett játékot, amennyiben van
                        try
                        {
                            _model.NewGame(); // indításkor új játékot kezdünk
                            _model.FirsPosition();
                        }
                        catch
                        {
                        }
                    }
                };
                activatableLifetime.Deactivated +=  (sender, args) =>
                {
                    if (args.Kind == ActivationKind.Background)
                    {

                        // elmentjük a jelenleg folyó játékot
                        try
                        {
                            _model.NewGame(); // indításkor új játékot kezdünk
                            _model.FirsPosition();
                            // Androidon az AppContext.BaseDirectory az alkalmazás adat könyvtára, ahova
                            // akár külön jogosultság nélkül is lehetne írni
                        }
                        catch
                        {
                        }
                    }
                };
                }
        }

        base.OnFrameworkInitializationCompleted();
    }
    #endregion

    #region ViewModel event handlers

    /// <summary>
    /// Új játék indításának eseménykezelője.
    /// </summary>
    private void ViewModel_NewGame(object? sender, EventArgs e)
    {
        _model.NewGame();
        _model.FirsPosition();
    }
    private void ViewModel_PauseGame(object? sender, EventArgs e)
    {

        _model?.PauseGame();

    }
    private void ViewModel_ExitGame(object? sender, EventArgs e)
    {

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.Shutdown();
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = null;
            
        }


    }

    #endregion
    #region Model event handlers

    /// <summary>
    /// Játék végének eseménykezelője.
    /// </summary>
    private async void Model_GameOver(object? sender, LabyrinthEventArgs e)
    {
        await Dispatcher.UIThread.InvokeAsync(async () =>
        {
            if (e.IsWon) // győzelemtől függő üzenet megjelenítése
            {
                await MessageBoxManager.GetMessageBoxStandard(
                        "Sudoku játék",
                        "Gratulálok, győztél!" + Environment.NewLine +
                         " lépést tettél meg és " +
                        TimeSpan.FromSeconds(e.GameTime).ToString("g") + " ideig játszottál.",
                        ButtonEnum.Ok, Icon.Info)
                    .ShowAsync();
            }
            else
            {
                await MessageBoxManager.GetMessageBoxStandard(
                        "Sudoku játék",
                        "Sajnálom, vesztettél, lejárt az idő!",
                        ButtonEnum.Ok, Icon.Info)
                    .ShowAsync();
            }
        });
    }

    #endregion
}
