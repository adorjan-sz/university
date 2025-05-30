﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PvsZWPF.ViewModelAndStuff
{
    public class Field : ViewModelBase
    {

        private bool _isEmpty;
        private bool _isZombie;
        private String _text = String.Empty;

        /// <summary>
        /// Zároltság lekérdezése, vagy beállítása.
        /// </summary>
        public Boolean IsEmpty
        {
            get { return _isEmpty; }
            set
            {
                if (_isEmpty != value)
                {
                    _isEmpty = value;
                    OnPropertyChanged();
                }
            }
        }
        public Boolean IsZombie
        {
            get { return _isZombie; }
            set
            {
                if (_isZombie != value)
                {
                    _isZombie = value;
                    OnPropertyChanged();
                }
            }
        }

        

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
