using ModelAndPersistence.Model;
using PvsZWPF.ViewModelAndStuff;
using System.Configuration;
using System.Data;
using System.Windows;
using ModelAndPersistence.Persistence;
namespace PvsZWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private GameModel _model;
        private ViewModel _viewModel;
        private MainWindow _view = null!;
        public App()
        {
            Startup += new StartupEventHandler(App_Startup);
        }
        private void App_Startup(object? sender, StartupEventArgs e)
        {
            // modell létrehozása
            _model = new GameModel(new DataAccess());
            _model.GameOver += new EventHandler(Over);
            

            // nézemodell létrehozása
            _viewModel = new ViewModel(_model);
            

            // nézet létrehozása
            _view = new MainWindow();
            _view.DataContext = _viewModel;
            //_view.Closing += new System.ComponentModel.CancelEventHandler(View_Closing); // eseménykezelés a bezáráshoz
            _view.Show();

            _model.NewGame();
        }
        private void Over(Object sender, EventArgs e)
        {
            MessageBox.Show("GameOver");
        }
    }

}
