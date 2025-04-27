using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelAndPersistence.Model;

namespace LocatorWPF.ViewModelAndStuff
{
    public class ViewModel:ViewModelBase
    {
        private GameModel _model;
        public ObservableCollection<Field> Fields { get; private set; }

        public ViewModel(GameModel model)
        {
            _model = model;
            _model.TableChanged += new EventHandler<TableEventArgs>(Update);
            // játéktábla létrehozása
            Fields = new ObservableCollection<Field>();
            for (Int32 i = 0; i < _model.Size; i++) // inicializáljuk a mezőket
            {
                for (Int32 j = 0; j < _model.Size; j++)
                {
                    Fields.Add(new Field
                    {
                        IsTarget = false,
                        IsVisible = false,
                        X = i,
                        Y = j,
                        StepCommand = new DelegateCommand(param =>
                        {
                            if (param is Tuple<Int32, Int32> position)
                                _model.SetBomb(position.Item1, position.Item2);
                        })
                        // ha egy mezőre léptek, akkor jelezzük a léptetést, változtatjuk a lépésszámot
                    });
                }
            }

           
        }

        private void Update(object? sender, TableEventArgs e)
        {
            Fields = new ObservableCollection<Field>();
            for (Int32 i = 0; i < _model.Size; i++) // inicializáljuk a mezőket
            {
                for (Int32 j = 0; j < _model.Size; j++)
                {
                    Fields.Add(new Field
                    {
                        IsTarget = e.table.Get(i,j),
                        IsVisible = e.table.GetVisible(i,j),
                        X = i,
                        Y = j,
                        StepCommand = new DelegateCommand(param =>
                        {
                            if (param is Tuple<Int32, Int32> position)
                                _model.SetBomb(position.Item1, position.Item2);
                        })
                        // ha egy mezőre léptek, akkor jelezzük a léptetést, változtatjuk a lépésszámot
                    });
                }
            }
            OnPropertyChanged(nameof(Fields));

        }
    }
}
