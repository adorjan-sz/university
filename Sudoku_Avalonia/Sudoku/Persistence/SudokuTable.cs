using System;

namespace ELTE.Sudoku.Persistence
{
    public class LabyrinthTable
    {
        #region Fields

        private Int32 _size;
        private Boolean[,] _fieldWall;


        #endregion

        #region Properties
        public Boolean this[int x, int y] { get { return _fieldWall[x, y]; } }
        public Int32 Size { get { return _size; } }
        #endregion
        #region Constructor
        public LabyrinthTable(Int32 size)
        {
            _size = size;
            _fieldWall = new Boolean[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    _fieldWall[i, j] = false;
                }
            }
        }
        #endregion
        #region Public methods
        public Boolean IsWall(Int32 x, Int32 y)
        {
            if (x < 0 || x >= _fieldWall.GetLength(0))
                throw new ArgumentOutOfRangeException(nameof(x), "The X coordinate is out of range.");
            if (y < 0 || y >= _fieldWall.GetLength(1))
                throw new ArgumentOutOfRangeException(nameof(y), "The Y coordinate is out of range.");

            return _fieldWall[x, y];
        }

        public void SetValue(Int32 x, Int32 y, Boolean isWall)
        {
            if (x < 0 || x >= _fieldWall.GetLength(0))
                throw new ArgumentOutOfRangeException(nameof(x), "The X coordinate is out of range.");
            if (y < 0 || y >= _fieldWall.GetLength(1))
                throw new ArgumentOutOfRangeException(nameof(y), "The Y coordinate is out of range.");


            _fieldWall[x, y] = isWall;

        }
        #endregion

    }
}
