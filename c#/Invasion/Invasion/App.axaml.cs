using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using Invasion.ViewModels;
using Invasion.Views;
using Game.Model;
using Game.Persistence;
using Avalonia.Controls;
using Avalonia.Data.Core.Plugins;
using Avalonia.Platform;
using System;
using Avalonia.Threading;
namespace Invasion;

public partial class App : Application
{
    private GameModel _model = null!;
    private InvasionViewModel _viewModel = null!;

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
        _model = new GameModel(new DataAccess(), new TimerInheritance());
        _model.GameOver += new EventHandler(Model_GameOver);


        // nézemodell létrehozása
        _viewModel = new InvasionViewModel(_model);
        _viewModel.NewGame += new EventHandler(ViewModel_NewGame);
        _viewModel.PauseGame += new EventHandler(ViewModel_PauseGame);
        //_viewModel.LoadGame += new EventHandler(ViewModel_LoadGame);
        //_viewModel.SaveGame += new EventHandler(ViewModel_SaveGame);

        // nézet létrehozása
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // asztali környezethez
            desktop.MainWindow = new MainWindow
            {
                DataContext = _viewModel

            };

            desktop.Startup += async (s, e) =>
            {
               _model.NewGame(); // indításkor új játékot kezdünk
                /*
                // betöltjük a felfüggesztett játékot, amennyiben van
                try
                {
                    await _model.LoadGameAsync(
                        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "SudokuSuspendedGame"));
                }
                catch { }*/
            };

            desktop.Exit += async (s, e) =>
            {/*
                // elmentjük a jelenleg folyó játékot
                try
                {
                    await _model.SaveGameAsync(
                        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "SudokuSuspendedGame"));
                    // mentés a felhasználó Documents könyvtárába, oda minden bizonnyal van jogunk írni
                }
                catch { }*/
            };
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            // mobil környezethez
            singleViewPlatform.MainView = new MainView
            {
                DataContext = _viewModel
            };

            if (Application.Current?.TryGetFeature<IActivatableLifetime>() is { } activatableLifetime)
            {
                activatableLifetime.Activated += async (sender, args) =>
                {/*
                    if (args.Kind == ActivationKind.Background)
                    {
                        // betöltjük a felfüggesztett játékot, amennyiben van
                        try
                        {
                            await _model.LoadGameAsync(
                                Path.Combine(AppContext.BaseDirectory, "SuspendedGame"));
                        }
                        catch
                        {
                        }
                    }*/
                };
                activatableLifetime.Deactivated += async (sender, args) =>
                {
                    if (args.Kind == ActivationKind.Background)
                    {   /*

                        // elmentjük a jelenleg folyó játékot
                        try
                        {
                            await _model.SaveGameAsync(
                                Path.Combine(AppContext.BaseDirectory, "SuspendedGame"));
                            // Androidon az AppContext.BaseDirectory az alkalmazás adat könyvtára, ahova
                            // akár külön jogosultság nélkül is lehetne írni
                        }
                        catch
                        {
                        }*/
                    }
                };
            }
        }

        base.OnFrameworkInitializationCompleted();
    }
    private async void Model_GameOver(object sender, EventArgs e)
    {
        await Dispatcher.UIThread.InvokeAsync(async () =>
        {
            await MessageBoxManager.GetMessageBoxStandard(
                        "Sudoku játék",
                        "Sajnálom, vesztettél",
                        ButtonEnum.Ok, Icon.Info)
                    .ShowAsync();
        });
    }
    private void ViewModel_NewGame(object sender, EventArgs e)
    {
        _model.NewGame();
    }
    private void ViewModel_PauseGame(object sender, EventArgs e)
    {
        _model.Pause();
    }
}
