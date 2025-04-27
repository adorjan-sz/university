using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;

using LocatorAvalonia.ViewModels;
using LocatorAvalonia.Views;
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
namespace LocatorAvalonia;

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

        // modell létrehozása
        _model = new GameModel(new DataAccess());
        _model.GameOver += new EventHandler<GameOverEventArgs>(Model_GameOver);
        

        // nézemodell létrehozása
        _viewModel = new MainViewModel(_model);
        
        _model.NewGame();
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
                ;
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
                        ;
                    }
                };
                activatableLifetime.Deactivated +=  (sender, args) =>
                {
                    if (args.Kind == ActivationKind.Background)
                    {

                        ;
                    }
                };
            }
        }

        base.OnFrameworkInitializationCompleted();
    }

    #endregion
    private async void Model_GameOver(object? sender, GameOverEventArgs e)
    {
        await Dispatcher.UIThread.InvokeAsync(async () =>
        {
            
                await MessageBoxManager.GetMessageBoxStandard(
                        "Locator játék",
                        "Gratulálok, győztél!" + Environment.NewLine +
                        "Összesen " + e.BombCount + " Bombát tettél le" ,
                        ButtonEnum.Ok, Icon.Info)
                    .ShowAsync();
            
        });
        _model.NewGame();
    }
}
