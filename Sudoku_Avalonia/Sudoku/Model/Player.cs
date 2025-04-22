using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELTE.Sudoku.Model
{

    public class Player
    {

        private int x, y;
        #region Propery
        public int X { get { return x; } set { x = value; } }
        public int Y { get { return y; } set { y = value; } }

        #endregion
        public Player(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

    }
}
