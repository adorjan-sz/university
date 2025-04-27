using ModelAndPersistence.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ModelAndPersistence.Model
{
    public class GameModel
    {
        private Table _table;
        private IDataAccess _dataAccess;
        private Random random;
        public int Row { get; set; }
        public int Column { get; set; }
        private System.Timers.Timer _timer;
        public int Nap { get; private set; }
        public int GameTime { get; private set; }
        public event EventHandler<TableChanged> TableUpdate;
        public event EventHandler GameOver;
        private bool IsGameOver;

        public GameModel(IDataAccess data)
        {
            _dataAccess = data;
            _table = _dataAccess.Load(5, 10);
            Row = _table.Row;
            Column = _table.Column;
            _timer = new System.Timers.Timer();
            _timer.Interval = 1000;
            _timer.Elapsed += new System.Timers.ElapsedEventHandler(TimeAdvenced);
            GameTime = 0;
            Nap = 75;
            random = new Random();


        }
        public void NewGame()
        {

            _table = _dataAccess.Load(5, 10);
            Row = _table.Row;
            Column = _table.Column;
            IsGameOver = false;
            GameTime = 0;
            Nap = 75;
            _timer.Start();
        }

        public void TimeAdvenced(Object? sender, ElapsedEventArgs e)
        {
            _table.AdvenceZombie();
            if (GameTime % 4 == 0) { Nap += 25; }
            if (GameTime % 4 == 0) { _table.ShootZombie(); }
            if (GameTime % 3 == 0)
            {
                int v = random.Next(10);
                
                switch (v)
                {
                    case 0:
                        int x = random.Next(Row);
                        while (!_table.set(x, Column - 1, new Zombie()))
                        {
                            x = random.Next(Row);
                        }
                        break;
                    case 1:
                        int x2 = random.Next(Row);
                        while (!_table.set(x2, Column - 1, new Zombie()))
                        {
                            x2 = random.Next(Row);
                        }
                        break;
                    case 2:
                        int x3 = random.Next(Row);
                        while (!_table.set(x3, Column - 1, new Zombie()))
                        {
                            x3 = random.Next(Row);
                        }
                        break;
                }
                
            }
            TableUpdate?.Invoke(this, new TableChanged(_table));
            if (_table.ZombieInTheHoouse())
            {
                GameOver?.Invoke(this,EventArgs.Empty);
                IsGameOver = true;
                _timer.Stop();
            }
        }
        public void SetPlant(int x, int y)
        {
            
            if (!IsGameOver && Nap >= 100 && _table.set(x, y, new Plant()))
            {
                Nap -= 100;
            }


        }
    }
}
