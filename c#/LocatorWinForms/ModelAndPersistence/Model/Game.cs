using ModelAndPersistence.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAndPersistence.Model
{
    public class GameModel
    {
        private Table _table;
        private int _bombCount;
        public int Size { get { return _table.Size; } }
        private IDataAccess _dataAccess;
        public event EventHandler<TableEventArgs>? TableChanged;
        public event EventHandler<GameOverEventArgs>? GameOver;
        public GameModel(IDataAccess data)
        {
            _dataAccess = data;
            _table = _dataAccess.Load(9);
            _bombCount = 0;
        }
        public void NewGame()
        {
            _table = _dataAccess.Load(9);
            _bombCount = 0;
            TableChanged?.Invoke(this, new TableEventArgs(_table));
        }
        public void SetBomb(int x, int y)
        {
            if(_table.PutBomb(x, y))
            {
                _bombCount++;
                
                if (_table.IsGameOver()) { 
                    GameOver?.Invoke(this,new GameOverEventArgs(_bombCount));
                }
            }
            TableChanged?.Invoke(this, new TableEventArgs(_table));

        }
    }
}
