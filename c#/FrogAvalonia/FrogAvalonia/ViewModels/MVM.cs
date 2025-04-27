using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using ModelAndPersistence.Model;
using ModelAndPersistence.Persistence;

namespace FrogAvalonia.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public event EventHandler? NewGame;

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
        private GameModel _model;
        public RelayCommand NewGameCommand { get; private set; }
        public int Size { get { return _model.Size; } }
        public int Cols
        {
            get
            {
                switch (_model.GameDifficulty)
                {
                    case GameDifficulty.Easy:

                        return 20;
                    case GameDifficulty.Medium:
                        return 16;
                    case GameDifficulty.Hard:
                        return 12; ;

                }
                return 0;
            }
        }
        public ObservableCollection<Field> Fields { get; set; }


        public MainViewModel(GameModel game)
        {
            _model = game;
            _model.TableChanged += new EventHandler<TableChangedEventArgs>(UpdateTable);
            NewGameCommand = new RelayCommand(OnNewGame);
            Fields = new ObservableCollection<Field>();
            for (Int32 i = 0; i < 9; i++) // inicializáljuk a mezőket
            {
                for (Int32 j = 0; j < _model.Size; j++)
                {
                    Fields.Add(new Field
                    {
                        IsType = 0,
                        X = i,
                        Y = j,
                        StepCommand = new RelayCommand<Tuple<Int32, Int32>>(position =>
                        {
                            if (position != null)
                                ;
                        })
                        // ha egy mezőre léptek, akkor jelezzük a léptetést, változtatjuk a lépésszámot
                    });
                }
            }

        }
        private void OnNewGame()
        {
            NewGame?.Invoke(this, EventArgs.Empty);
        }

        private int IEntityToInt(IEntity entity) {
            switch (entity)
            {
                case Empty:
                    return 0;
                case Car:
                    return 1;
                case Safety:
                    return 2;
                
            }
            return -1;
        }
        private void UpdateTable(object sender, TableChangedEventArgs e) {

            Fields = new ObservableCollection<Field>();
            for (Int32 i = 0; i < 9; i++) // inicializáljuk a mezőket
            {
                for (Int32 j = 0; j < _model.Size; j++)
                {
                    Fields.Add(new Field
                    {
                        IsType = IEntityToInt(e.table[i, j]),
                        X = i,
                        Y = j,
                        StepCommand = new RelayCommand<Tuple<Int32, Int32>>(position =>
                        {
                            if (position != null)
                                ;
                        })
                        // ha egy mezőre léptek, akkor jelezzük a léptetést, változtatjuk a lépésszámot
                    });
                }
            }
            OnPropertyChanged(nameof(Fields));
            OnPropertyChanged(nameof(Cols));
        }
    }
}
