using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAndPersistence.Persistence
{
    public interface IDataAccess
    {
        public Table EasyLoad();
        public Table MediumLoad();
        public Table HardLoad();

    }
    public class DataAccess : IDataAccess
    {
        public Table EasyLoad()
        {
            return new Table(5);
        }
        public Table MediumLoad()
        {
            return new Table(7);
        }
        public Table HardLoad()
        {
            return new Table(9);
        }
    }
}
