using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;

namespace BattleOfShapesAvalonia.ViewModels
{
    public class Field : ViewModelBase
    {

        private Boolean _empty;
        private Boolean _isPlayer1;


        /// <summary>
        /// Zároltság lekérdezése, vagy beállítása.
        /// </summary>
        public Boolean IsEmpty
        {
            get { return _empty; }
            set
            {
                if (_empty != value)
                {
                    _empty = value;
                    OnPropertyChanged();
                }
            }
        }
        public Boolean IsPlayer1
        {
            get { return _isPlayer1; }
            set
            {
                if (_isPlayer1 != value)
                {
                    _isPlayer1 = value;
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
