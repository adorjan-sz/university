using Attack.ViewModel;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Threading;
using ModelAndStuff.Model;
using ModelAndStuff.Persistence;

namespace Attack
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        private GameModel _model = null!;
        private ViewModel.ViewModel _viewModel = null!;
        private MainWindow _view = null!;
        private DispatcherTimer _timer = null!;
        public App()
        {
            Startup += new StartupEventHandler(App_Startup);
        }


        

        private void App_Startup(object? sender, StartupEventArgs e)
        {
            // modell létrehozása

            _model = new GameModel(new DataAcces());
            //_model.GameOver += new EventHandler<LabyrinthEventArgs>(Model_GameOver);
            //_model.NewGame();

            // nézemodell létrehozása
            _viewModel = new ViewModel.ViewModel(_model);
           _viewModel.NewGame += new EventHandler(ViewModel_NewGame);
           /* _viewModel.ExitGame += new EventHandler(ViewModel_ExitGame);
            _viewModel.PauseGame += new EventHandler(ViewModel_PauseGame);
*/
            //this. += new KeyEventHandler(KeyCheck);


            // nézet létrehozása
            _view = new MainWindow();
            _view.DataContext = _viewModel;
            //_view.Closing += new System.ComponentModel.CancelEventHandler(View_Closing); // eseménykezelés a bezáráshoz
            _view.Show();

        }
        private void ViewModel_NewGame(object? sender, System.EventArgs e)
        {
            _model.NewGame();
        }

    }

}
