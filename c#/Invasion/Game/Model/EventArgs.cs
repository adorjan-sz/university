using Game.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Model
{
    public class BoardEventArgs: EventArgs
    {
        public Board Board;
        public BoardEventArgs(Board board)
        {
            Board = board;
        }
    }
    public class DataEventArgs : EventArgs
    {
        public int GameTime;
        public int Health;
        public DataEventArgs(int GameTime,int Healt)
        {
            this.GameTime = GameTime;
            this.Health = Healt;
        }
    }


}
