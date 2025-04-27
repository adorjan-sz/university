using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Persistence
{
    public interface IDataAccess
    {
        public Board Load();
    }
    public class DataAccess:IDataAccess
    {
        public Board Load()
        {
            Board board = new Board(10);
            return board;
        }
        public DataAccess()
        {

        }
    }
}
