using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAndPersistence.Persistence
{
    public class Table
    {
        private bool[,] _table;
        private bool[,] _visible;
        private Random random;
        public bool Get(int x,int y)
        {
            return _table[x,y];
        }
        public int Size { get; init; }
        public bool GetVisible(int x,int y)
        {
            return _visible[x,y];
        }

        public Table(int n) {
            Size = n;
            _table = new bool[Size,Size];
            _visible = new bool[Size,Size];
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++) {
                    _table[i, j] = false; 
                    _visible[i, j] = false;
                }
            }
            random = new Random();  
            SetUpTable();
        }
        public void SetUpTable()
        {
            int count = 0;
            int x;
            int y;
            while (count < Size * 2)
            {
                x = random.Next(Size);
                y = random.Next(Size);
                if (!_table[x, y])
                {
                    _table[x, y] = true;
                    count++;
                }
            }
        }
        public bool PutBomb(int x, int y) {

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    
                    _visible[i, j] = false;
                }
            }
            bool WasThereBomb = false;

            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++) {
                    if (x + i < Size && x + i >= 0 && y + j < Size && y + j >= 0) {
                        if (_table[x + i, y + j])
                        {
                            _table[x + i, y + j] = false;
                            WasThereBomb = true;
                        }
                    }
                }
            }
            for (int i = -2; i < 3; i++)
            {
                for (int j = -2; j < 3; j++)
                {
                    if (x + i < Size && x + i >= 0 && y + j < Size && y + j >= 0)
                    {
                        
                        
                            _visible[x + i, y + j] = true;
                            
                        
                    }
                }
            }
            return WasThereBomb;
        }
        public bool IsGameOver() {
            for (int i = 0; i < Size; i++)
            {
                for(int j =0; j < Size; j++)
                {
                    if (_table[i,j] == true)
                        return false;
                }
            }
            return true;
        }
    }
}
