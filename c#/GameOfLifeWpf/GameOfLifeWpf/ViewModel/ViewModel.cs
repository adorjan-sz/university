using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using GameModel.Model;
using GameModel.Persistence;

namespace GameOfLifeWpf.ViewModel
{
    /// <summary>
    /// Nézetmodell ősosztály típusa.
    /// </summary>
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
        private LifeGameModel _model;
        public int Size { get; set; }
        public ObservableCollection<Field> Fields { get; set; }

        public DelegateCommand PauseGameCommand { get; private set; }

        public event EventHandler GamePaused;
        public ViewModel(LifeGameModel model)
        {
            
            _model = model;
            Size = _model.TableSize;
            _model.TableChanged += new EventHandler<TableChangedEventArgs>(TableChanged);

            PauseGameCommand = new DelegateCommand(param => { GamePaused?.Invoke(this, EventArgs.Empty); });
            SetUp();
            //_model.NewGame();
        }
        private void TableChanged(object? sender, TableChangedEventArgs e) {
            Size= _model.TableSize;
            Fields = new ObservableCollection<Field>();
            for (int i = 0; i < Size; i++)
                {
                    for(int j =0; j < Size; j++)
                    {
                    Fields.Add(new Field
                    {
                        IsAlive = e.table.IsAlive(i, j),
                        X = i,
                        Y = j,
                        StepCommand = new DelegateCommand(param =>
                        {
                            if (param is Tuple<int, int> position)
                                _model.Set(position.Item1, position.Item2);
                        })
                    });
                }
            }
            OnPropertyChanged(nameof(Fields));
            
        }
       

        private void SetUp()
        {
            
            Fields = new ObservableCollection<Field>();
            for (int i = 0; i < _model.TableSize; i++)
            {
                for (int j = 0; j < _model.TableSize; j++)
                {
                    Fields.Add(new Field
                    {
                        IsAlive = false,
                        X = i,
                        Y = j,
                        StepCommand = new DelegateCommand(param =>
                        {
                            if (param is Tuple<int, int> position)
                                _model.Set(position.Item1, position.Item2);
                        })
                    });

                }
            }
        }
    }
}
