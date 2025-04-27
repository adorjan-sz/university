using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Persistence;
namespace Game.Model
{
    public class GameModel
    {
        private Board? _board;
        private IDataAccess _dataAccess;
        private int time;
        private bool _isPaused;
        private bool IsGameOver;
        private ITimer timer;
        public event EventHandler<BoardEventArgs>? BoardChanged;
        public event EventHandler<DataEventArgs>? DataChanged;
        public event EventHandler? GameOver;
        public Board Board { get { return _board; } }
        public int GameTime
        {
            get { return time; }
        }
        public GameModel(IDataAccess dataAccess,ITimer timer)
        {
            _dataAccess = dataAccess;

            this.timer = timer;
            timer.Interval = 1000;
            timer.Elapsed += new EventHandler(AdvenceTime);
        }
        public void NewGame()
        {
            time = 0;
            
            _board = _dataAccess.Load();
            _board.GenerateEnemy();
            IsGameOver = false;
            timer.Start();
        }
        public void AdvenceTime(Object? sender, EventArgs e)
        {
            
            if (IsGameOver) // ha már vége, nem folytathatjuk
                return;

            time++;
            _board?.AdvenceEnemy();
            _board?.GenerateEnemy();
            _board.Defend();
            _board.IsEnemyAtTheEnd();
            //boardchanged esemény kiváltása

            BoardChanged?.Invoke(this, new BoardEventArgs(_board));
            DataChanged?.Invoke(this, new DataEventArgs(time, _board.Health));
            //ha vége a játéknak
            if (_board.Health==0)
            {
                IsGameOver = true;
                timer.Stop();
                GameOver?.Invoke(this, EventArgs.Empty);
                
            }
        }
        public void Pause()
        {
            
            timer.Stop();
            if (_isPaused)
            {
                timer.Start();
                _isPaused = false;
            }
            else
            {
                _isPaused = true;
                timer.Stop();
            }
        }
        public void SetPlayer(int x, int y)
        {   
            if(!IsGameOver&& !_isPaused)
            {
                _board.PlacePlayer(x, y);
                BoardChanged?.Invoke(this, new BoardEventArgs(_board));
            }
            
           
        }

    }
}
