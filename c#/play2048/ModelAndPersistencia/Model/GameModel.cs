using ModelAndPersistencia.Persistence;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAndPersistencia.Model
{
    public class GameModel
    {
        private Table? _table;
        private IDataAccess _access;
        public event EventHandler<TableEventArgs>? TableChanged;
        public event EventHandler GameOver;
        public bool showOver;
        public int TableSize { get; set; }

        public GameModel(IDataAccess access)
        {
            _access = access;
            _table = _access.Load();
            TableSize = _table.Size;

        }
        public void NewGame()
        {
            _table = _access.Load();
            showOver = true;
            TableChanged?.Invoke(this,new TableEventArgs(_table));
            
        }
        public int getValue(int x, int y) {
            return _table.GetValue(x,y);
        }
        private void IsOver()
        {
            if (showOver)
            {
                for (int i = 0; i < TableSize; i++)
                {
                    for (int j = 0; j < TableSize; j++)
                    {
                        if (_table.GetValue(i, j) >= 8)
                        {
                            GameOver?.Invoke(this, EventArgs.Empty);
                            showOver = false;
                        }
                    }
                }
            }
            
            


        }
        public void MoveDown()
        {
            _table?.MoveDown();
            TableChanged?.Invoke(this, new TableEventArgs(_table));

            IsOver();

        }
        public void MoveRight()
        {
            _table?.MoveRight();
            TableChanged?.Invoke(this, new TableEventArgs(_table));
            IsOver();
        }
        public void MoveLeft()
        {
            _table?.MoveLeft();
            TableChanged?.Invoke(this, new TableEventArgs(_table));
            IsOver();
        }
        public void MoveUp()
        {
            _table?.MoveUp();
            TableChanged?.Invoke(this, new TableEventArgs(_table));
            IsOver();
        }

    }
}
