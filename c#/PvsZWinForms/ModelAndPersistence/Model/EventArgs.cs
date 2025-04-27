using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelAndPersistence.Persistence;
namespace ModelAndPersistence.Model
{
    public class TableChanged :EventArgs
    {
        public Table table;
        public TableChanged(Table table) {
            this.table = table;
        }
    }
}
