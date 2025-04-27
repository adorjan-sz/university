using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameModel.Persistence
{
    public class Table
    {
        private bool[,] _table;
        public bool IsAlive(int x, int y)
        {
            return _table[x, y];
        }
        public int Size { get; init; }

        public Table(int n)
        {
            Size = n;
            _table = new bool[Size, Size];
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    _table[i, j] = false;
                }
            }

        }
        public void set(int x, int y)
        {
            _table[x, y] = !_table[x, y];
        }

        public void NextRound()
        {
            int rows = _table.GetLength(0);
            int cols = _table.GetLength(1);
            bool[,] newGrid = new bool[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    int liveNeighbors = CountLiveNeighbors(i, j);

                    // Apply the rules
                    if (_table[i, j])
                    {
                        newGrid[i, j] = liveNeighbors == 2 || liveNeighbors == 3;
                    }
                    else
                    {
                        newGrid[i, j] = liveNeighbors == 3;
                    }
                }
            }

            _table = newGrid;
        }

        private int CountLiveNeighbors(int row, int col)
        {
            int rows = _table.GetLength(0);
            int cols = _table.GetLength(1);
            int count = 0;

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0)
                        continue;

                    int neighborRow = row + i;
                    int neighborCol = col + j;

                    if (neighborRow >= 0 && neighborRow < rows && neighborCol >= 0 && neighborCol < cols && _table[neighborRow, neighborCol])
                    {
                        count++;
                    }
                }
            }

            return count;
        }



    }
}
