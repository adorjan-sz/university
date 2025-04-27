using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Persistence;

namespace RubikWPF.ViewModelAndStuff
{
    public class Field : ViewModelBase
    {
        private int _colour;
        

        /// <summary>
        /// Zároltság lekérdezése, vagy beállítása.
        /// </summary>
        public int colour 
        {
            get { return _colour; }
            set
            {
                if (_colour != value)
                {
                    _colour = value;
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
        public DelegateCommand? StepCommand { get; set; }
    }
}
