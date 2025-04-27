using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAndStuff.Persistence
{
    public interface IDataAcces
    {
        public GameBoard Load(int n);
        
    }
    public class DataAcces : IDataAcces
    {
        public GameBoard Load(int n)
        {
            GameBoard gameBoard = new GameBoard(n);
            return gameBoard;
        }
    }
}
