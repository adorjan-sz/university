using ModelAndStuff.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAndStuff.Model
{
    public class BoardChangedEventArgs : EventArgs
    {
        public GameBoard board;
        public BoardChangedEventArgs(GameBoard board) {
         this.board = board;
        }
    }
    public class TurnEventArgs : EventArgs
    {
        public bool isLeftPlayerTurn;
        public int id;
        public TurnEventArgs(bool isLeftPlayerTurn,int id)
        {
            this.isLeftPlayerTurn = isLeftPlayerTurn;
            this.id = id;
        }
    }


}
