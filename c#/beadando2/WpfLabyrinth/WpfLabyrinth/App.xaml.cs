using Labyrinth.Model;
using Labyrinth.Persistence;
using Microsoft.Win32;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using WpfLabyrinth.ViewModel;

namespace WpfLabyrinth
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        #region Fields
        
        private LabyrinthGameModel _model = null!;
        private LabyrinthViewModel _viewModel = null!;
        private MainWindow _view = null!;
        private DispatcherTimer _timer = null!;

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

        #region Application event handlers

        private void App_Startup(object? sender, StartupEventArgs e)
        {
            // modell létrehozása
            
            _model = new LabyrinthGameModel(new LabyrinthFileDataAccess());
            _model.GameOver += new EventHandler<LabyrinthEventArgs>(Model_GameOver);
            //_model.NewGame();
            
            // nézemodell létrehozása
            _viewModel = new LabyrinthViewModel(_model);
            _viewModel.NewGame += new EventHandler(ViewModel_NewGame);
            _viewModel.ExitGame += new EventHandler(ViewModel_ExitGame);
            _viewModel.PauseGame += new EventHandler(ViewModel_PauseGame);
            
            //this. += new KeyEventHandler(KeyCheck);


            // nézet létrehozása
            _view = new MainWindow();
            _view.DataContext = _viewModel;
            _view.Closing += new System.ComponentModel.CancelEventHandler(View_Closing); // eseménykezelés a bezáráshoz
            _view.Show();

            // időzítő létrehozása
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += new EventHandler(Timer_Tick);
            _timer.Start();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            
            _model.AdvanceTime();
        }

        #endregion

        #region View event handlers

        /// <summary>
        /// Nézet bezárásának eseménykezelője.
        /// </summary>
        private void View_Closing(object? sender, CancelEventArgs e)
        {
            Boolean restartTimer = _timer.IsEnabled;

            _timer.Stop();

            if (MessageBox.Show("Biztos, hogy ki akar lépni?", "Sudoku", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {
                e.Cancel = true; // töröljük a bezárást

                if (restartTimer) // ha szükséges, elindítjuk az időzítőt
                    _timer.Start();
            }
        }

        #endregion

        #region ViewModel event handlers

        /// <summary>
        /// Új játék indításának eseménykezelője.
        /// </summary>
        /// 
        private void ViewModel_PauseGame(object? sender, EventArgs e)
        {

            _timer.Stop();


            MessageBox.Show("Szünet",
                            "Labirintus játék",
                            MessageBoxButton.OK,
                            MessageBoxImage.Asterisk);
            _timer.Start();

        }
        private void ViewModel_NewGame(object? sender, EventArgs e)
        {
            _model.NewGame();
            _model.FirsPosition();
            _timer.Start();
        }




        /// <summary>
        /// Játékból való kilépés eseménykezelője.
        /// </summary>
        private void ViewModel_ExitGame(object? sender, System.EventArgs e)
        {
            _view.Close(); // ablak bezárása
        }

        #endregion

        #region Model event handlers

        /// <summary>
        /// Játék végének eseménykezelője.
        /// </summary>
        private void Model_GameOver(object? sender, LabyrinthEventArgs e)
        {
            _timer.Stop();

            
                MessageBox.Show("Gratulálok, győztél!",
                                "Labirintus játék",
                                MessageBoxButton.OK,
                                MessageBoxImage.Asterisk);
        }
        
                    #endregion
            }

        }
