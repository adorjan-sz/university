using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAndPersistencia.Persistence
{
    public class DataAccess : IDataAccess
    {
        public Table Load()
        {
            return new Table(4);
        }
    }
}
