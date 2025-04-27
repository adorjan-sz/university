using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ModelAndPersistence.Model;
using ModelAndPersistence.Persistence;
namespace BattleOfShapesAvalonia.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        #region Fields
        public int WindowSize
        {
            get { return Size * 100; }
        }
        private GameModel _model; // modell
        public int Size { get { return _model.TableSize; } }
        public bool IsPlayerOneComes { get { return _model.IsPlayer1Comes; } }
        public int PlayerOneCount { get { return _model.Player1Count; } }
        public int PlayerTwoCount { get { return _model.Player2Count; } }


        #endregion
        public ObservableCollection<Field> Fields { get; set; }
        public ObservableCollection<Field> NextFields { get; set; }

        public Boolean IsGameEasy
        {
            get { return _model.GameDifficulty == GameDifficulty.Easy; }
            set
            {
                if (_model.GameDifficulty == GameDifficulty.Easy)
                    return;

                _model.GameDifficulty = GameDifficulty.Easy;
                OnPropertyChanged(nameof(IsGameEasy));
                OnPropertyChanged(nameof(IsGameMedium));
                OnPropertyChanged(nameof(IsGameHard));
            }
        }

        /// <summary>
        /// Közepes nehézségi szint állapotának lekérdezése.
        /// </summary>
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
        /// 
        public RelayCommand NewGameCommand { get; private set; }
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

        public MainViewModel(GameModel model)
        {
            // játék csatlakoztatása
            NewGameCommand = new RelayCommand(OnNewGame);
            _model = model;
            _model.TableChanged += new EventHandler<TableEventArgs>(UpdateTable);
            _model.NextTableChanged += new EventHandler<NextTableEventArgs>(UpdateNextTable);
            _model.CountChanged += new EventHandler<CountChangedEventArgs>(UpdateCount);


            // játéktábla létrehozása
            Fields = new ObservableCollection<Field>();
            for (Int32 i = 0; i < _model.TableSize; i++) // inicializáljuk a mezőket
            {
                for (Int32 j = 0; j < _model.TableSize; j++)
                {
                    Fields.Add(new Field
                    {
                        IsEmpty = true,
                        IsPlayer1 = false,

                        X = i,
                        Y = j,
                        StepCommand = new RelayCommand<Tuple<Int32, Int32>>(param =>
                        {
                            if (param is Tuple<Int32, Int32> position)
                                _model.Set(position.Item1, position.Item2);
                        })
                        // ha egy mezőre léptek, akkor jelezzük a léptetést, változtatjuk a lépésszámot
                    });
                }
            }
            NextFields = new ObservableCollection<Field>();
            for (Int32 i = 0; i < 3; i++) // inicializáljuk a mezőket
            {
                for (Int32 j = 0; j < 3; j++)
                {
                    NextFields.Add(new Field
                    {
                        IsEmpty = true,
                        IsPlayer1 = false,

                        X = i,
                        Y = j,
                        StepCommand = new RelayCommand<Tuple<Int32, Int32>>(param =>
                        {
                            ;
                        })
                        // ha egy mezőre léptek, akkor jelezzük a léptetést, változtatjuk a lépésszámot
                    });
                }
            }


        }
        public event EventHandler? NewGame;
        private void OnNewGame()
        {
            NewGame?.Invoke(this, EventArgs.Empty);
        }
        private void UpdateNextTable(object sender, NextTableEventArgs e)
        {
            NextFields = new ObservableCollection<Field>();
            for (Int32 i = 0; i < 3; i++) // inicializáljuk a mezőket
            {
                for (Int32 j = 0; j < 3; j++)
                {
                    NextFields.Add(new Field
                    {
                        IsEmpty = e.NextTable[i, j].IsEmpty,
                        IsPlayer1 = e.NextTable[i, j].IsPlayer1,

                        X = i,
                        Y = j,
                        StepCommand = new RelayCommand<Tuple<Int32, Int32>>(param =>
                        {
                            ;
                        })
                        // ha egy mezőre léptek, akkor jelezzük a léptetést, változtatjuk a lépésszámot
                    });
                }
            }
            OnPropertyChanged(nameof(NextFields));
        }
        private void UpdateCount(object sender, CountChangedEventArgs e)
        {

        }
        private void UpdateTable(object sender, TableEventArgs e)
        {
            Fields = new ObservableCollection<Field>();
            for (Int32 i = 0; i < _model.TableSize; i++) // inicializáljuk a mezőket
            {
                for (Int32 j = 0; j < _model.TableSize; j++)
                {
                    Fields.Add(new Field
                    {
                        IsEmpty = e.table[i, j].IsEmpty,
                        IsPlayer1 = e.table[i, j].IsPlayer1,

                        X = i,
                        Y = j,
                        StepCommand = new RelayCommand<Tuple<Int32, Int32>>(param =>
                        {
                            if (param is Tuple<Int32, Int32> position)
                                _model.Set(position.Item1, position.Item2);
                        })
                        // ha egy mezőre léptek, akkor jelezzük a léptetést, változtatjuk a lépésszámot
                    });
                }
            }
            OnPropertyChanged(nameof(Fields));
            OnPropertyChanged(nameof(PlayerTwoCount));
            OnPropertyChanged(nameof(PlayerOneCount));
            OnPropertyChanged(nameof(IsPlayerOneComes));
            OnPropertyChanged(nameof(Size));
        }

    }

}
