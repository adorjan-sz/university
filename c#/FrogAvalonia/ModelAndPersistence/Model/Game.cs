using ModelAndPersistence.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAndPersistence.Model
{
    public enum GameDifficulty { Easy, Medium, Hard }
    public class GameModel
    {
        private GameDifficulty _gameDifficulty;
        public GameDifficulty GameDifficulty
        {
            get { return _gameDifficulty; }
            set { _gameDifficulty = value; }
        }
        private IDataAccess dataAccess;
        private Table _table;
        private System.Timers.Timer _timer;
        public int Size { get {  return _table.Size; } }
        public event EventHandler<TableChangedEventArgs>? TableChanged;

        public GameModel(IDataAccess db)
        {
            dataAccess = db;
            _gameDifficulty = GameDifficulty.Easy;
            _table = dataAccess.Load(20);
            _timer = new System.Timers.Timer();
            _timer.Interval = 1000;
            _timer.Elapsed += new System.Timers.ElapsedEventHandler(AdvenceTime);
            _timer.Start();
        }
        public void NewGame()
        {
            
            switch (_gameDifficulty)
            {
                case GameDifficulty.Easy:
                    _table = dataAccess.Load(20); break;
                case GameDifficulty.Medium:
                    _table = dataAccess.Load(16); break;
                case GameDifficulty.Hard:
                    _table = dataAccess.Load(12); break;

            }
            
            TableChanged?.Invoke(this,new TableChangedEventArgs(_table));
        }
        private void AdvenceTime(object sender, System.Timers.ElapsedEventArgs e) {
            _table.Advence();
            TableChanged?.Invoke(this, new TableChangedEventArgs(_table));
        }


    }

}
