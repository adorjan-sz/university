using System.Configuration;
using System.Data;
using System.Windows;
using Game.Model;
using Game.Persistence;
using RubikWPF.ViewModelAndStuff;

namespace RubikWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Fields

        private GameModel _model = null!;
        private ViewModel _viewModel = null!;
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
            
         

            // nézemodell létrehozása
            _viewModel = new ViewModel(_model);

            _model.NewGame();
            // nézet létrehozása
            _view = new MainWindow();
            _view.DataContext = _viewModel;
            //_view.Closing += new System.ComponentModel.CancelEventHandler(View_Closing); // eseménykezelés a bezáráshoz
            _view.Show();
        }
    }

}
