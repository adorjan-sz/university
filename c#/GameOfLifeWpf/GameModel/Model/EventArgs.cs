using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameModel.Persistence;

namespace GameModel.Model
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
