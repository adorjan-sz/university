using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelAndPersistencia.Persistence;

namespace ModelAndPersistencia.Model
{
    public class TableEventArgs: EventArgs
    {
        public Table table {  get;  }
        public TableEventArgs(Table table)
        {
            this.table = table;
        }
    }
}
