using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;

using FrogAvalonia.ViewModels;
using FrogAvalonia.Views;
using System;
using ModelAndPersistence.Model;
using ModelAndPersistence.Persistence;


using System.ComponentModel;
using System.IO;

using Avalonia.Platform.Storage;
using Avalonia.Platform;
using Avalonia.Threading;

namespace FrogAvalonia;

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
        
        _model.NewGame();

        // nézemodell létrehozása
        _viewModel = new MainViewModel(_model);
        _viewModel.NewGame += new EventHandler(ViewModel_NewGame);

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

                // betöltjük a felfüggesztett játékot, amennyiben van
               
            };

            desktop.Exit += async (s, e) =>
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
                activatableLifetime.Activated += async (sender, args) =>
                {
                    if (args.Kind == ActivationKind.Background)
                    {
                        
                    }
                };
                activatableLifetime.Deactivated += async (sender, args) =>
                {
                    if (args.Kind == ActivationKind.Background)
                    {

                        // elmentjük a jelenleg folyó játékot
                       
                    }
                };
            }
        }

        base.OnFrameworkInitializationCompleted();
    }
    private void ViewModel_NewGame(object? sender, EventArgs e)
    {
        _model.NewGame();
    }

    #endregion
}
