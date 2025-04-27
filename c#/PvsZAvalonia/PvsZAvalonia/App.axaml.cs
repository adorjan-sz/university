using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using ModelAndPersistence.Model;
using ModelAndPersistence.Persistence;
using PvsZAvalonia.ViewModels;
using PvsZAvalonia.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using Avalonia.Controls;
using Avalonia.Platform;


using System.ComponentModel;
using System.IO;

using Avalonia.Platform.Storage;

using Avalonia.Threading;

namespace PvsZAvalonia;

public partial class App : Application
{

    private GameModel _model = null!;
    private MainViewModel _viewModel = null!;
    
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
        _model = new GameModel(new DataAccess());
        _model.GameOver += new EventHandler(Over);
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
                        _model.NewGame();
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
    private async void Over(object? sender, EventArgs e)
    {
        await Dispatcher.UIThread.InvokeAsync(async () =>
        {
            await MessageBoxManager.GetMessageBoxStandard(
            "Plants vs Zombies", "Game Over", ButtonEnum.Ok, Icon.Info).ShowAsync();
        } );
    }
}
