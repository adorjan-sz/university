using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attack.ViewModel
{
    public class AttackField : ViewModelBase
    {
        private int x;
        private int y;
        private bool isPlayer;
        private int id;
        private bool isAlive;
        private bool isLeftPlayer;
        public bool IsLeftPlayer
        {
            get { return isLeftPlayer; }
            set
            {
                isLeftPlayer = value;
                OnPropertyChanged();
            }
        }

        public bool IsAlive
        {
            get { return isAlive; }
            set
            {
                isAlive = value;
                OnPropertyChanged();
            }
        }
        public int X
        {
            get { return x; }
            set
            {
                x = value;
                OnPropertyChanged();
            }
        }
        public int Y
        {
            get { return y; }
            set
            {
                y = value;
                OnPropertyChanged();
            }
        }
        public bool IsPlayer
        {
            get { return isPlayer; }
            set
            {
                isPlayer = value;
                OnPropertyChanged();
            }
        }
        public int Id
        {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged();
            }
        }
        public Tuple<Int32, Int32> XY
        {
            get { return new(X, Y); }
        }
        public DelegateCommand? StepCommand { get; set; }
    }
}
