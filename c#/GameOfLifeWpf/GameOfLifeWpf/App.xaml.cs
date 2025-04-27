using GameModel.Model;
using GameModel.Persistence;
using GameOfLifeWpf.ViewModel;
using System.Configuration;
using System.Data;
using System.Windows;

namespace GameOfLifeWpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private LifeGameModel _model;
        private ViewModel.ViewModel _viewModel;
        private MainWindow _view = null!;
        public App()
        {
            Startup += new StartupEventHandler(App_Startup);
        }
        private void App_Startup(object? sender, StartupEventArgs e)
        {
            // modell létrehozása
            _model = new LifeGameModel(new DataAccess());
            //_model.GameOver += new EventHandler<SudokuEventArgs>(Model_GameOver);
            

            // nézemodell létrehozása
            _viewModel = new ViewModel.ViewModel(_model);
            _viewModel.GamePaused += new EventHandler(StopStart);
            _model.NewGame();

            // nézet létrehozása
            _view = new MainWindow();
            _view.DataContext = _viewModel;
            //_view.Closing += new System.ComponentModel.CancelEventHandler(View_Closing); // eseménykezelés a bezáráshoz
            _view.Show();
        }
        private void StopStart(Object sender, EventArgs e) {
         _model.StartStop();
        }

    }

}
