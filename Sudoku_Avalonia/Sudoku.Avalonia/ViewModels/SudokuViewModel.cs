using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.Input;
using ELTE.Sudoku.Model;
using ELTE.Sudoku.Persistence;

namespace ELTE.Sudoku.Avalonia.ViewModels
{
    public class LabyrinthViewModel : ViewModelBase
    {
        #region Fields
        private LabyrinthGameModel _model;
        private int _gridRows;
        private int _gridColumns;
        public int GridRows
        {
            get { return _gridRows; }
            set { _gridRows = value; OnPropertyChanged(); }
        }
        public int GridColumns
        {
            get { return _gridColumns; }
            set { _gridColumns = value; OnPropertyChanged(); }
        }

        #endregion

            #region Properties
        public ICommand DPressed { get; }
        public ICommand SPressed { get; }
        public ICommand APressed { get; }
        public ICommand WPressed { get; }
        public ICommand HandleKeyPressCommand { get; }
        public ICommand PauseCommand { get; }
        public ICommand NewGameCommand { get; }
        public ICommand ExitCommand { get; }
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

            HandleKeyPressCommand = new RelayCommand(() => { Debug.WriteLine("Key Pressed Command executed"); });
            DPressed = new RelayCommand(OnDPressed);
            SPressed = new RelayCommand(OnSPressed);
            APressed = new RelayCommand(OnAPressed);
            WPressed = new RelayCommand(OnWPressed);



            // parancsok kezelése
            PauseCommand = new RelayCommand(OnGamePaused);
            NewGameCommand = new RelayCommand(OnNewGame);
            ExitCommand = new RelayCommand(OnExitGame);
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
                        DoNothing = new RelayCommand(Nothing)

                        // ha egy mezőre léptek, akkor jelezzük a léptetést, változtatjuk a lépésszámot
                    });

                }

            }
            _model.FirsPosition();

        }

        #endregion
        #region Private methods
        private void OnDPressed()
        {
            _model.Step(Direction.Right);
        }
        private void OnSPressed()
        {
            _model.Step(Direction.Down);
        }
        private void OnAPressed()
        {
            _model.Step(Direction.Left);
        }
        private void OnWPressed()
        {
            _model.Step(Direction.Up);
        }
        private void Nothing()
        {
            ;
        }


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

            if (_model.Table.Size != GridRows)
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
                            IsPlayer = i == _model.Table.Size - 1 && j == 0,
                            DoNothing = new RelayCommand(Nothing)


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
