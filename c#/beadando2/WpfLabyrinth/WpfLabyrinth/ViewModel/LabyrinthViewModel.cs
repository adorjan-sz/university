using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Labyrinth;
using Labyrinth.Model;
using Labyrinth.Persistence;
using System.Diagnostics;
using System.Windows.Input;

namespace WpfLabyrinth.ViewModel
{
    public class LabyrinthViewModel : ViewModelBase
    {
        #region Fields
        private LabyrinthGameModel _model;
        private int GridRows;
        private int GridColumns;
        public ICommand HandleKeyPressCommand { get; }
        #endregion

        #region Properties
        public DelegateCommand? dPressed { get; private set; }

        public DelegateCommand? sPressed { get; private set; }
        public DelegateCommand? aPressed { get; private set; }
        public DelegateCommand? wPressed { get; private set; }
        public DelegateCommand? NewGameCommand { get; private set; }
        public DelegateCommand? PauseCommand { get; private set; }
        public DelegateCommand? ExitCommand { get; private set; }
        public ObservableCollection<LabyrinthField> Fields { get; private set; }
        public string GameTime { get { return TimeSpan.FromSeconds(_model.GameTime).ToString("g"); } }
        public Boolean IsGameEasy
        {
            get
            {
                return _model.GameDifficulty == GameDifficulty.Easy;
            }
            set
            {
                if (_model.GameDifficulty == GameDifficulty.Easy)
                {
                    return;
                }
                _model.GameDifficulty = GameDifficulty.Easy;
                OnPropertyChanged(nameof(IsGameEasy));
                OnPropertyChanged(nameof(IsGameMedium));
                OnPropertyChanged(nameof(IsGameHard));
            }
        }
        public Boolean IsGameMedium
        {
            get { return _model.GameDifficulty == GameDifficulty.Medium; }
            set
            {
                if (_model.GameDifficulty == GameDifficulty.Medium)
                    return;

                _model.GameDifficulty = GameDifficulty.Medium;
                OnPropertyChanged(nameof(IsGameEasy));
                OnPropertyChanged(nameof(IsGameMedium));
                OnPropertyChanged(nameof(IsGameHard));
            }
        }

        /// <summary>
        /// Magas nehézségi szint állapotának lekérdezése.
        /// </summary>
        public Boolean IsGameHard
        {
            get { return _model.GameDifficulty == GameDifficulty.Hard; }
            set
            {
                if (_model.GameDifficulty == GameDifficulty.Hard)
                    return;

                _model.GameDifficulty = GameDifficulty.Hard;
                OnPropertyChanged(nameof(IsGameEasy));
                OnPropertyChanged(nameof(IsGameMedium));
                OnPropertyChanged(nameof(IsGameHard));
            }
        }
        #endregion
        #region Events

        /// <summary>
        /// Új játék eseménye.
        /// </summary>
        public event EventHandler? NewGame;

        public event EventHandler? PauseGame;
        /// <summary>
        /// Játékból való kilépés eseménye.
        /// </summary>
        public event EventHandler? ExitGame;

        #endregion
        #region Constructors

        /// <summary>
        /// Sudoku nézetmodell példányosítása.
        /// </summary>
        /// <param name="model">A modell típusa.</param>
        /// 
        
        public LabyrinthViewModel(LabyrinthGameModel model)
        {
            // játék csatlakoztatása
            _model = model;
            _model.PlayerMoved += new EventHandler<LabyrinthPlayerEventArgs>(Model_FieldChanged);
            _model.GameAdvanced += new EventHandler<LabyrinthEventArgs>(Model_GameAdvanced);
            
            GridColumns = _model.Table.Size;
            GridRows = _model.Table.Size;
            HandleKeyPressCommand = new DelegateCommand(_ =>
            {
                Debug.WriteLine("Key Pressed Command executed");
            });
            dPressed = new DelegateCommand(p =>  _model.Step(Direction.Right));
            aPressed = new DelegateCommand(p => _model.Step(Direction.Left));
            sPressed = new DelegateCommand(p => _model.Step(Direction.Down));
            wPressed = new DelegateCommand(p => _model.Step(Direction.Up));

            // parancsok kezelése
            PauseCommand = new DelegateCommand(p => OnGamePaused());
            NewGameCommand = new DelegateCommand(param => OnNewGame());
            
            ExitCommand = new DelegateCommand(param => OnExitGame());
            
            // játéktábla létrehozása
            Fields = new ObservableCollection<LabyrinthField>();
            for (Int32 i = 0; i < _model.Table.Size; i++) // inicializáljuk a mezőket
            {
                for (Int32 j = 0; j < _model.Table.Size; j++)
                {

                    Fields.Add(new LabyrinthField
                    {
                        IsVisible = false,

                        X = i,
                        Y = j,
                        IsWall = _model.Table[i, j],
                        IsPlayer = i == _model.Table.Size - 1 && j == 0,
                        

                        // ha egy mezőre léptek, akkor jelezzük a léptetést, változtatjuk a lépésszámot
                    });
                    
                }

            }
            _model.FirsPosition();

        }

        #endregion
        #region Private methods

     

        /// <summary>
        /// Játék léptetése eseménykiváltása.
        /// </summary>
        /// <param name="x">A lépett mező X koordinátája.</param>
        /// <param name="y">A lépett mező Y koordinátája.</param>
        

        #endregion

        #region Game event handlers

        /// <summary>
        /// Játékmodell mező megváltozásának eseménykezelője.
        /// </summary>
        private void Model_FieldChanged(object? sender, LabyrinthPlayerEventArgs e)
        {
            
            if(_model.Table.Size != GridRows)
            {
                GridColumns = _model.Table.Size;
                GridRows = _model.Table.Size;

                // játéktábla létrehozása
                Fields = new ObservableCollection<LabyrinthField>();
                for (Int32 i = 0; i < _model.Table.Size; i++) // inicializáljuk a mezőket
                {
                    for (Int32 j = 0; j < _model.Table.Size; j++)
                    {

                        Fields.Add(new LabyrinthField
                        {
                            IsVisible = false,
                            X = i,
                            Y = j,
                            IsWall = _model.Table.IsWall(i, j),
                            IsPlayer = i == _model.Table.Size - 1 && j == 0



                            // ha egy mezőre léptek, akkor jelezzük a léptetést, változtatjuk a lépésszámot
                        });
                    }

                }
                
                OnPropertyChanged(nameof(GridRows));
                OnPropertyChanged(nameof(GridColumns));
            }
            // mező frissítése
            
            foreach (LabyrinthField LF in Fields)
            {
                if (e.VisibleCoords.Contains(LF.XY))
                {
                    LF.IsVisible = true;
                    LF.IsPlayer = LF.X == _model.player.X && LF.Y == _model.player.Y;
                }
                else
                {
                    LF.IsVisible = false;
                }
            }
            OnPropertyChanged(nameof(Fields));
            

        }

        /// <summary>
        /// Játék végének eseménykezelője.
        /// </summary>
        

        /// <summary>
        /// Játék előrehaladásának eseménykezelője.
        /// </summary>
        private void Model_GameAdvanced(object? sender, LabyrinthEventArgs e)
        {
            OnPropertyChanged(nameof(GameTime));
        }


        #endregion
        #region Event methods

        /// <summary>
        /// Új játék indításának eseménykiváltása.
        /// </summary>
        /// 
        private void OnGamePaused()
        {
            PauseGame?.Invoke(this, EventArgs.Empty);
        }
        private void OnNewGame()
        {
            NewGame?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Játékból való kilépés eseménykiváltása.
        /// </summary>
        private void OnExitGame()
        {
            ExitGame?.Invoke(this, EventArgs.Empty);
        }


        #endregion
        
    }
}
