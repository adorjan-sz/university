using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Persistence
{
    public enum Colour
    {
        Y,R,G,B,N
    }
    public enum Direction
    {
        TOPLEFT, TOPRIGHT, BOTTOMLEFT, BOTTOMRIGHT
    }
    public class Table
    {
        public Colour this[int x,int y]
        {
            get { return _table[x, y]; }
        }
        private Random _random;
        private Colour[,] _table;
        public int Size = 5;
        public Table()
        {
            _random = new Random();
            _table = new Colour[5, 5];
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    _table[i, j] = Colour.N;
                }
            }
            for (int i = 1; i < 4; i++) {
                _table[0,i] = Colour.R;
            }
            for (int i = 1; i < 4; i++)
            {
                _table[4, i] = Colour.B;
            }
            for (int i = 1; i < 4; i++)
            {
                _table[i, 0] = Colour.Y;
            }
            for (int i = 1; i < 4; i++)
            {
                _table[i, 4] = Colour.G;
            }
            int r;
            for (int i = 0; i < 10; i++)
            {
                r = _random.Next(4);
                switch (r)
                {
                    case 0:
                        Move(Direction.TOPRIGHT); break;
                    case 1:
                        Move(Direction.TOPLEFT); break;
                    case 2:
                        Move(Direction.BOTTOMRIGHT); break;
                    case 3:
                        Move(Direction.BOTTOMLEFT); break;
                }
            }

        }
        public void Move(Direction direction) {
            if (direction == Direction.TOPLEFT)
            {
                Colour temp0x1 = _table[0, 1];
                Colour temp0x2 = _table[0, 2];
                _table[0, 1] = _table[1, 0];
                _table[0, 2] = _table[2, 0];
                _table[1, 0] = temp0x1;
                _table[2, 0] = temp0x2;
            }
            else if (direction == Direction.TOPRIGHT)
            {
                Colour temp0x2 = _table[0, 2];
                Colour temp0x3 = _table[0, 3];
                _table[0, 2] = _table[2, Size - 1];
                _table[0, 3] = _table[1, Size - 1];
                _table[2, Size - 1] = temp0x2;
                _table[1, Size - 1] = temp0x3;
            }
            else if (direction == Direction.BOTTOMLEFT) {
                Colour temp0x2 = _table[2, 0];
                Colour temp0x3 = _table[3, 0];
                _table[2,0] = _table[Size-1,2];
                _table[3,0] = _table[Size - 1, 1];
                _table[Size - 1, 2] = temp0x2;
                _table[Size - 1, 1] = temp0x3;
            }
            else if(direction == Direction.BOTTOMRIGHT) 
            {
                 Colour  tempLx2 = _table[Size-1,2] ;
                 Colour tempLx3 = _table[Size - 1, 3];
                _table[Size - 1, 2] = _table[2, Size - 1];
                _table[Size - 1, 3] = _table[3, Size - 1];
                _table[2, Size - 1] = tempLx2;
                _table[3, Size - 1] = tempLx3;

            }

        }



    }
}
