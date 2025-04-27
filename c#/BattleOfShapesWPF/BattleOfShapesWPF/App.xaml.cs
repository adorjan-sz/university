using System.Configuration;
using System.Data;
using System.Windows;
using BattleOfShapesWPF.ViewModelAndStuff;
using ModelAndPersistence.Model;
using ModelAndPersistence.Persistence;



namespace BattleOfShapesWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Fields

        private GameModel _model = null!;
        private MainViewModel _viewModel = null!;
        private MainWindow _view = null!;

        #endregion

        #region Constructors

        /// <summary>
        /// Alkalmazás példányosítása.
        /// </summary>
        public App()
        {
            Startup += new StartupEventHandler(App_Startup);
        }

        #endregion

        

        private void App_Startup(object? sender, StartupEventArgs e)
        {
            // modell létrehozása
            _model = new GameModel(new DataAccess());
            _model.GameOver += new EventHandler (Model_GameOver);


            // nézemodell létrehozása
            _viewModel = new MainViewModel(_model);
            _viewModel.NewGame += new EventHandler(ViewModel_NewGame); 

            _model.NewGame();
            // nézet létrehozása
            _view = new MainWindow();
            _view.DataContext = _viewModel;
            //_view.Closing += new System.ComponentModel.CancelEventHandler(View_Closing); // eseménykezelés a bezáráshoz
            _view.Show();
        }
        private void ViewModel_NewGame(object? sender, EventArgs e)
        {
            _model.NewGame();
        }
        private void Model_GameOver(object? sender, EventArgs e)
        {
            if (_viewModel.PlayerOneCount > _viewModel.PlayerTwoCount)
            {
                MessageBox.Show("Gratulálok, Egyes győztél!" + Environment.NewLine +
                                   "Összesen " + _viewModel.PlayerOneCount + " lépést tettél meg és "
                                   ,
                                   "Sudoku játék",
                                   MessageBoxButton.OK,
                                   MessageBoxImage.Asterisk);
            }
            else
            {
                MessageBox.Show("Gratulálok, kettes győztél!" + Environment.NewLine +
                                   "Összesen " + _viewModel.PlayerTwoCount + " lépést tettél meg és "
                                   ,
                                   "Sudoku játék",
                                   MessageBoxButton.OK,
                                   MessageBoxImage.Asterisk);
            }
        }

    }

}
