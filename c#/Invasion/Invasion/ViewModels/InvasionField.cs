using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invasion.ViewModels
{
    public class InvasionField : ViewModelBase
    {
        private bool _isEnemy;
        private bool _isField;
        public bool IsEnemy { 
            get { return _isEnemy; }
            set { _isEnemy = value;
                OnPropertyChanged();
            }
        }
        public bool IsField { 
            get { return _isField; }
            set
            {
                _isField = value;
                OnPropertyChanged();
            }
        }
        public InvasionField()
        {
            
        }
        public Int32 X { get; set; }

        /// <summary>
        /// Függőleges koordináta lekérdezése, vagy beállítása.
        /// </summary>
        public Int32 Y { get; set; }

        /// <summary>
        /// Koordináta lekérdezése.
        /// </summary>
        public Tuple<Int32, Int32> XY
        {
            get { return new(X, Y); }
        }

        /// <summary>
        /// Lépés parancs lekérdezése, vagy beállítása.
        /// </summary>
        public RelayCommand<Tuple<Int32, Int32>>? StepCommand { get; set; }
    }
}
