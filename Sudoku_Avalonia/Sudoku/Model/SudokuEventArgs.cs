using System;

namespace ELTE.Sudoku.Model
{

    public class LabyrinthEventArgs : EventArgs
    {

        private Int32 _gameTime;

        private Boolean _isWon;


        public Int32 GameTime { get { return _gameTime; } }



        public Boolean IsWon { get { return _isWon; } }


        public LabyrinthEventArgs(Boolean isWon, Int32 gameTime)
        {
            _isWon = isWon;

            _gameTime = gameTime;
        }
    }
}
