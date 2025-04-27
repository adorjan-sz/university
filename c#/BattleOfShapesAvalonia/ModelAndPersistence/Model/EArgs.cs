using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelAndPersistence.Persistence;

namespace ModelAndPersistence.Model
{
    public class TableEventArgs: EventArgs
    {
        public IEntity[,] table;
        public TableEventArgs(IEntity[,] table)
        {
            this.table = table;
        }

    }
    public class NextTableEventArgs : EventArgs
    {
        public IEntity[,] NextTable;
        public NextTableEventArgs(IEntity[,] NextTable)
        {
            this.NextTable = NextTable;
        }
    }
    public class CountChangedEventArgs:EventArgs {
        int Player1Count;
            int Player2Count;
        
        public CountChangedEventArgs(int Player1Count, int Player2Count)
        {
            this.Player1Count = Player1Count;
            this.Player2Count = Player2Count;
           
        }
    }
}
