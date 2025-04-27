using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;

using BattleOfShapesAvalonia.ViewModels;
using BattleOfShapesAvalonia.Views;
using System;
using ModelAndPersistence.Model;
using ModelAndPersistence.Persistence;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;

using System.ComponentModel;
using System.IO;

using Avalonia.Platform.Storage;
using Avalonia.Platform;
using Avalonia.Threading;


namespace BattleOfShapesAvalonia;

public partial class App : Application
{
    #region Fields

    private GameModel _model = null!;
    private MainViewModel _viewModel = null!;

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

       
        
        _model = new GameModel(new DataAccess());
        _model.GameOver += new EventHandler(Model_GameOver);


        // nézemodell létrehozása
        _viewModel = new MainViewModel(_model);
        _viewModel.NewGame += new EventHandler(ViewModel_NewGame);

        _model.NewGame();
        // nézet létrehozása
       

        // nézet létrehozása
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // asztali környezethez
            desktop.MainWindow = new MainWindow
            {
                DataContext = _viewModel
            };

            desktop.Startup +=  (s, e) =>
            {
                _model.NewGame(); // indításkor új játékot kezdünk

                
            };

            desktop.Exit +=  (s, e) =>
            {
                // elmentjük a jelenleg folyó játékot
                
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
                activatableLifetime.Activated +=  (sender, args) =>
                {
                    if (args.Kind == ActivationKind.Background)
                    {
                        
                    }
                };
                activatableLifetime.Deactivated += async (sender, args) =>
                {
                    if (args.Kind == ActivationKind.Background)
                    {

                    }
                };
            }
        }

        base.OnFrameworkInitializationCompleted();
    }

    #endregion

    

    /// <summary>
    /// Új játék indításának eseménykezelője.
    /// </summary>
    private void ViewModel_NewGame(object? sender, EventArgs e)
    {
        _model.NewGame();
    }
    private async void Model_GameOver(object? sender, EventArgs e)
    {
        await Dispatcher.UIThread.InvokeAsync(async () =>
        {
            
             if (_viewModel.PlayerOneCount > _viewModel.PlayerTwoCount)
            {
                await MessageBoxManager.GetMessageBoxStandard("Gratulálok, Egyes győztél!" + Environment.NewLine +
                                   "Összesen " + _viewModel.PlayerOneCount + " lépést tettél meg és "
                                   ,
                                   "Sudoku játék",
                                   ButtonEnum.Ok, Icon.Info).ShowAsync();
            }
            else
            {

                await MessageBoxManager.GetMessageBoxStandard("Gratulálok, Egyes győztél!" + Environment.NewLine +
                                   "Összesen " + _viewModel.PlayerOneCount + " lépést tettél meg és "
                                   ,
                                   "Sudoku játék",
                                   ButtonEnum.Ok, Icon.Info).ShowAsync();
            }
        });
    }

}
