using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Model;
using Game.Persistence;

namespace RubikWPF.ViewModelAndStuff
{
    public class ViewModel : ViewModelBase
    {
        #region Fields
        public int Size { get { return _model.TableSize; } }

        private GameModel _model; // modell
        public ObservableCollection<Field> Fields { get; set; }

        #endregion

        public ViewModel(GameModel model)
        {
            // játék csatlakoztatása
            _model = model;
            _model.TableChanged += new EventHandler<TableChangedEventArgs>(Update);
            

            
            // játéktábla létrehozása
            Fields = new ObservableCollection<Field>();
            for (Int32 i = 0; i < _model.TableSize; i++) // inicializáljuk a mezőket
            {
                for (Int32 j = 0; j < _model.TableSize; j++)
                {
                    Fields.Add(new Field
                    {
                        colour = 0,
                        X = i,
                        Y = j,
                        StepCommand = new DelegateCommand(param =>
                        {
                           /* if (param is Tuple<Int32, Int32> position)
                                StepGame(position.Item1, position.Item2);*/
                        })
                        // ha egy mezőre léptek, akkor jelezzük a léptetést, változtatjuk a lépésszámot
                    });
                }
            }
            OnPropertyChanged(nameof(Fields));

            
        }
        private int ColourToInt(Colour colour)
        {
            switch (colour)
            {
                case Colour.N:
                    return 0;
                    break;
                case Colour.R:
                    return 1;
                    break;
                case Colour.Y:
                    return 2;
                    break;
                case Colour.B:
                    return 3;
                    break;
                case Colour.G:
                    return 4;
                    break;
            }
                    return -1;
            }

        private void Update(object sender, TableChangedEventArgs e) {
            Fields = new ObservableCollection<Field>();
            for (Int32 i = 0; i < _model.TableSize; i++) // inicializáljuk a mezőket
            {
                for (Int32 j = 0; j < _model.TableSize; j++)
                {
                    Fields.Add(new Field
                    {
                        colour = ColourToInt(e.table[i,j]),
                        X = i,
                        Y = j,
                        StepCommand = new DelegateCommand(param =>
                        {

                            if (param is Tuple<Int32, Int32> position)
                            {
                                _model.Rotate(position.Item1, position.Item2);
                            }
                        })
                        // ha egy mezőre léptek, akkor jelezzük a léptetést, változtatjuk a lépésszámot
                    });
                }
            }
            OnPropertyChanged(nameof(Fields));
        }

    }
}
