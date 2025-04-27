using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocatorWPF.ViewModelAndStuff
{
    public class Field : ViewModelBase
    {
        private Boolean _isTarget;
        private Boolean _isVisible;

        /// <summary>
        /// Zároltság lekérdezése, vagy beállítása.
        /// </summary>
        public Boolean IsTarget
        {
            get { return _isTarget; }
            set
            {
                if (_isTarget != value)
                {
                    _isTarget = value;
                    OnPropertyChanged();
                }
            }
        }
        public Boolean IsVisible
        {
            get { return _isVisible; }
            set
            {
                if (_isVisible != value)
                {
                    _isVisible = value;
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
