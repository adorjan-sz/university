using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrogAvalonia.ViewModels
{
    public class Field : ViewModelBase
    {
        private int _type;
       

        /// <summary>
        /// Zároltság lekérdezése, vagy beállítása.
        /// </summary>
        /// 
        public bool IsEmpty { get { return _type == 0; } }
        public bool IsSafety { get { return _type == 2; } }
        public bool IsCar { get { return _type == 1; } }
        public int IsType
        {
            get { return _type; }
            set
            {
                if (_type != value)
                {
                    _type = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Felirat lekérdezése, vagy beállítása.
        /// </summary>
       

        /// <summary>
        /// Vízszintes koordináta lekérdezése, vagy beállítása.
        /// </summary>
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
