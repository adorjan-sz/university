using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelAndPersistence.Persistence;

namespace ModelAndPersistence.Model
{
    public class TableEventArgs :EventArgs
    {
        public Table table { get; init; }
        public TableEventArgs(Table table)
        {
            this.table = table;
        }
    }
    public class GameOverEventArgs : EventArgs
    {
        public int BombCount { get; init; }
        public GameOverEventArgs(int bombCount)
        {
            this.BombCount = bombCount;
        }
    }
}
