using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ModelAndPersistence.Model;

namespace PvsZWPF.ViewModelAndStuff
{
    public class ViewModel : ViewModelBase
    {
        private GameModel _model;
        public ObservableCollection<Field> Fields { get; set; }
        public ViewModel(GameModel model)
        {
            _model = model;
            
            _model.TableUpdate += new EventHandler<TableChanged>(Update);



            Fields = new ObservableCollection<Field>();
            for (Int32 i = 0; i < _model.Row; i++) // inicializáljuk a mezőket
            {
                for (Int32 j = 0; j < _model.Column; j++)
                {
                    Fields.Add(new Field
                    {
                        IsEmpty = true,
                        IsZombie = false,
                        X = i,
                        Y = j,
                        StepCommand = new DelegateCommand(param =>
                        {
                            if (param is Tuple<Int32, Int32> position)
                                _model.SetPlant(position.Item1, position.Item2);
                        })
                        // ha egy mezőre léptek, akkor jelezzük a léptetést, változtatjuk a lépésszámot
                    });
                }
            }
            OnPropertyChanged();
            

        }
        private void Update(Object sender, TableChanged e) {
            Fields = new ObservableCollection<Field>();
            for (Int32 i = 0; i < _model.Row; i++) // inicializáljuk a mezőket
            {
                for (Int32 j = 0; j < _model.Column; j++)
                {
                    Fields.Add(new Field
                    {
                        IsEmpty = e.table.GetEntity(i,j).IsEmpty,
                        IsZombie = e.table.GetEntity(i, j).IsZombie,
                        X = i,
                        Y = j,
                        StepCommand = new DelegateCommand(param =>
                        {
                            if (param is Tuple<Int32, Int32> position)
                                _model.SetPlant(position.Item1, position.Item2);
                        })
                        // ha egy mezőre léptek, akkor jelezzük a léptetést, változtatjuk a lépésszámot
                    });
                }
            }
            OnPropertyChanged(nameof(Fields));
        }
        

    }
    }

