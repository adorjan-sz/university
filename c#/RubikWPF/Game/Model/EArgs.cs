using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Persistence;
namespace Game.Model
{
   public class TableChangedEventArgs : EventArgs
    {
        public Table table;
        public TableChangedEventArgs(Table table)
        {
            this.table = table;
        }
    }
}
