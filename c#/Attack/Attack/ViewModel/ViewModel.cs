using ModelAndStuff.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;


namespace Attack.ViewModel
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Nézetmodell ősosztály példányosítása.
        /// </summary>
        protected ViewModelBase() { }

        /// <summary>
        /// Tulajdonság változásának eseménye.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Tulajdonság változása ellenőrzéssel.
        /// </summary>
        /// <param name="propertyName">Tulajdonság neve.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] String? propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    public class ViewModel : ViewModelBase
    {
        GameModel _model;
        private int Size;
        public DelegateCommand? NewGameCommand { get; private set; }
        public event EventHandler? NewGame;
        public int WhichPlyar;
        public string WhichPlayer
        {
            get
            {
                return WhichPlyar.ToString();
            }
        }
        private bool IsLeftPlayerTurn;
        public bool Turn
        {
            get
            {
                return IsLeftPlayerTurn;
            }
        }

        public Boolean IsGame4
        {
            get
            {
                return _model.Size == 4;
            }
            set
            {
                if (_model.Size == 4)
                {
                    return;
                }
                _model.Size = 4;
                OnPropertyChanged(nameof(IsGame4));
                OnPropertyChanged(nameof(IsGame6));
                OnPropertyChanged(nameof(IsGame8));
            }
        }
        public Boolean IsGame6
        {
            get
            {
                return _model.Size == 6;
            }
            set
            {
                if (_model.Size == 6)
                {
                    return;
                }
                _model.Size = 6;
                OnPropertyChanged(nameof(IsGame4));
                OnPropertyChanged(nameof(IsGame6));
                OnPropertyChanged(nameof(IsGame8));
            }
        }
        public Boolean IsGame8
        {
            get
            {
                return _model.Size == 8;
            }
            set
            {
                if (_model.Size == 8)
                {
                    return;
                }
                _model.Size = 8;
                OnPropertyChanged(nameof(IsGame4));
                OnPropertyChanged(nameof(IsGame6));
                OnPropertyChanged(nameof(IsGame8));
            }
        }



        public ObservableCollection<AttackField> Fields { get; private set; }
        public ViewModel(GameModel model) {
            _model = model;
            _model.BoardChanged += new EventHandler<BoardChangedEventArgs>(UpdateBoard);
            _model.TurnChanged += new EventHandler<TurnEventArgs>(UpdateTurn);
            NewGameCommand = new DelegateCommand(param => OnNewGame());
            _model.NewGame();
            

        }
        private void UpdateBoard(object? sender, BoardChangedEventArgs e)
        {
            if(Fields == null)
            {
                Fields = new ObservableCollection<AttackField>();
                Size = e.board.Size;
                for (int i = 0; i < Size; i++)
                {
                    for (int j = 0; j < Size; j++)
                    {
                        Fields.Add(new AttackField() { X = i, Y = j, 
                            IsPlayer = e.board.GetElement(i, j).IsPlayer, 
                            Id = e.board.GetElement(i, j).Id,
                            IsAlive= e.board.GetElement(i,j).IsAlive,
                        IsLeftPlayer = e.board.GetElement(i,j).IsLeftPlayer,
                            StepCommand = new DelegateCommand(param =>
                            {
                                if (param is Tuple<Int32, Int32> position)
                                    _model.Move(position.Item1, position.Item2);
                            })
                        });

                    }
                }
            }
            else if(Size != e.board.Size) 
            {
                OnPropertyChanged(nameof(IsGame4));
                OnPropertyChanged(nameof(IsGame6));
                OnPropertyChanged(nameof(IsGame8));
                Fields = new ObservableCollection<AttackField>();
                Size = e.board.Size;
                for (int i = 0; i < Size; i++)
                {
                    for (int j = 0; j < Size; j++)
                    {
                        Fields.Add(new AttackField()
                        {
                            X = i,
                            Y = j,
                            IsPlayer = e.board.GetElement(i, j).IsPlayer,
                            Id = e.board.GetElement(i, j).Id,
                            IsAlive = e.board.GetElement(i, j).IsAlive,
                            IsLeftPlayer = e.board.GetElement(i, j).IsLeftPlayer,
                            StepCommand = new DelegateCommand(param =>
                            {
                                if (param is Tuple<Int32, Int32> position)
                                    _model.Move(position.Item1, position.Item2);
                            })
                        });
                    }
                }
            }
            else
            {
                Fields = new ObservableCollection<AttackField>();
                for (int i = 0; i < Size; i++)
                {
                    for (int j = 0; j < Size; j++)
                    {
                        
                        Fields.Add(new AttackField()
                        {
                            X = i,
                            Y = j,
                            IsPlayer = e.board.GetElement(i, j).IsPlayer,
                            Id = e.board.GetElement(i, j).Id,
                            IsAlive = e.board.GetElement(i, j).IsAlive,
                            IsLeftPlayer = e.board.GetElement(i, j).IsLeftPlayer,
                            StepCommand = new DelegateCommand(param =>
                            {
                                if (param is Tuple<Int32, Int32> position)
                                    _model.Move(position.Item1, position.Item2);
                            })
                        });
                    }
                }
            }
            OnPropertyChanged(nameof(Fields));
        }
        private void UpdateTurn(object? sender, TurnEventArgs e)
        {
            WhichPlyar = e.id;
            IsLeftPlayerTurn = e.isLeftPlayerTurn;
            OnPropertyChanged(nameof(Turn));
            OnPropertyChanged(nameof(WhichPlayer));

        }
        private void OnNewGame()
        {
            NewGame?.Invoke(this, EventArgs.Empty);
        }
    }
}
