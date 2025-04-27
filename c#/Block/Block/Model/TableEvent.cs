using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Block.Model
{
    public class TableEventArgs: EventArgs
    {
        BlockType[,] Table;
        public BlockType this[int i, int j]
        {
            get { return Table[i, j]; }
        }
        public TableEventArgs(BlockType[,] Table)
        {
            this.Table = Table;   
        }
    }
    public class CurrentEventArgs : EventArgs
    {
        public BlockType Current;
        public CurrentEventArgs(BlockType Current)
        {
            this.Current = Current;
        }
    }
    public class GameOverEventArgs : EventArgs
    {
        public bool IsGameOver;
        public GameOverEventArgs(bool isGameOver)
        {
            IsGameOver = isGameOver;

        }
    }
    public class ScoreEventArgs : EventArgs
    {
        public int Score;
        public ScoreEventArgs(int score)
        {
            Score = score;
        }
    }
}
