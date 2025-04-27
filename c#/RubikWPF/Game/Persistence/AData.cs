using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Persistence
{
    public interface IDataAccess
    {
        public Table Load();
    }
    public class DataAccess : IDataAccess
    {
        public Table Load() { return new Table(); }
    }
}
