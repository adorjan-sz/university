using Game.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Model
{
    public class GameModel
    {
        private Table _table;
        private IDataAccess _access;
        public event EventHandler<TableChangedEventArgs> TableChanged;
        public int TableSize { get { return _table.Size; }  }
        public GameModel(IDataAccess db)
        {
            _access = db;
            _table = db.Load();
        }
        public void NewGame()
        {
            _table = _access.Load();
            TableChanged.Invoke(this, new TableChangedEventArgs(_table));

        }
        public void Rotate(int i,int j) {
            if (i == 0 && j == 0)
            {
                _table.Move(Direction.TOPLEFT);
            }
            else if (i == 0 && j == TableSize - 1)
            {
                _table.Move(Direction.TOPRIGHT);

            }
            else if (i == TableSize - 1 && j == 0)
            {
                _table.Move(Direction.BOTTOMLEFT);

            }
            else if (i == TableSize - 1 && j == TableSize - 1)
            {
                _table.Move(Direction.BOTTOMRIGHT);

            }
            TableChanged.Invoke(this, new TableChangedEventArgs(_table));
        }
    }
}
