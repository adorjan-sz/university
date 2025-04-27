using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAndStuff.Persistence
{
    public class GameBoard
    {
        
        int n;
        public int Size { get { return n; } }
        private IFieldElement[,] _table;


        public IFieldElement GetElement(int x, int y)
        {
            return _table[x, y];
        }
        public void SetElement(int x, int y, IFieldElement element)
        {
            _table[x, y] = element;
        }
        public void RefreshCoord()
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    _table[i, j].X = i;
                    _table[i, j].Y = j;
                }
            }
        }
        public GameBoard(int N)
        {
             n = N;
            _table = new IFieldElement[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    _table[i, j] = new Field();
                    _table[i, j].X = i;
                    _table[i, j].Y = j;
                }
            }
            {
                //left player
                _table[n-1, 0] = new Player(4, true);
                _table[n-1, 0].X = n-1;
                _table[n - 1, 0].Y = 0;
                _table[n-1, 1] = new Player(3, true);
                _table[n - 1, 1].X = n-1;
                _table[n-1, 1].Y = 1;
                _table[n - 2, 0] = new Player(1, true);
                _table[n - 2, 0].X = n - 2;
                _table[n - 2, 0].Y = 0;
                _table[n - 2, 1] = new Player(2, true);
                _table[n - 2, 1].X = n - 2;
                _table[n - 2, 1].Y = 1;
                //right player
                _table[0, n - 1] = new Player(4, false);
                _table[0, n-1].X = 0;
                _table[0, n-1].Y = n-1;
                _table[1, n-1] = new Player(1, false);
                _table[1, n - 1].X = 1;
                _table[1, n-1].Y = n-1;
                _table[0, n - 2] = new Player(3, false);
                _table[0, n - 2].X = 0;
                _table[0, n - 2].Y = n - 2;
                _table[1, n - 2] = new Player(2, false);
                _table[1, n - 2].X = 1;
                _table[1, n - 2].Y = n - 2;
            }
        }
    }
}
