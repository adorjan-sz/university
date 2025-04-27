using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAndPersistencia.Persistence
{
    public interface IDataAccess
    {
        public Table Load();
    }
}
