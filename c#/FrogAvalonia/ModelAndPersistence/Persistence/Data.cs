using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAndPersistence.Persistence
{
   public interface IDataAccess
    {
        public Table Load(int n);
    }
    public class DataAccess : IDataAccess
    {
        public Table Load(int n)
        {
            return new Table(n);
        }
    }
}
