using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfLabyrinth.ViewModel
{
    public class LabyrinthField : ViewModelBase
    {
        private Boolean _isVisible;
        private Boolean _isPlayer;
        private Boolean _isWall;
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
        public Boolean IsPlayer
        {
            get
            {
                return (_isPlayer);
            }
            set
            {
                if (_isPlayer != value)
                {
                    _isPlayer = value;
                    OnPropertyChanged();
                }
            }
        }
        public Boolean IsWall
        {
            get
            {
                return (_isWall);
            }
            set
            {
                if (_isWall != value)
                {
                    _isWall = value;
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

        

    }
}
