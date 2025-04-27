using System.Configuration;
using System.Data;
using System.Windows;
using ModelAndPersistence.Model;
using ModelAndPersistence.Persistence;

namespace LocatorWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        private GameModel _model = null!;
        private ViewModelAndStuff.ViewModel _viewModel;
        private MainWindow _view = null!;

        public App()
        {
            Startup += new StartupEventHandler(App_Startup);
        }
        private void App_Startup(object? sender, StartupEventArgs e)
        {
            // modell létrehozása
            _model = new GameModel(new DataAccess());
            _model.GameOver += new EventHandler<GameOverEventArgs>(Model_GameOver);
            

            // nézemodell létrehozása
            _viewModel = new ViewModelAndStuff.ViewModel(_model);
            _model.NewGame();

            // nézet létrehozása
            _view = new MainWindow();
            _view.DataContext = _viewModel;
            //_view.Closing += new System.ComponentModel.CancelEventHandler(View_Closing); // eseménykezelés a bezáráshoz
            _view.Show();
        }
        private void Model_GameOver(object? sender, GameOverEventArgs e)
        {
            MessageBox.Show("Gratulálok, győztél!" + Environment.NewLine +
                               "Összesen " + e.BombCount + " Bombát tettél le ",
                                
                               "Bomba játék",
                               MessageBoxButton.OK,
                               MessageBoxImage.Asterisk);
            _model.NewGame();
        }

    }

}
