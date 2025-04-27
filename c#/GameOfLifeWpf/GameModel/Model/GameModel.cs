using GameModel.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace GameModel.Model
{
    public class LifeGameModel
    {   
        
        private IDataAccess _dataAccess;
        private Table _table;
        private System.Timers.Timer timer;
        public bool IsGoing;
        public bool IsAlive(int x, int y)
        {
            return _table.IsAlive(x, y);
        }
        public int TableSize {  get { return _table.Size; } }    
        public event EventHandler<TableChangedEventArgs>? TableChanged;
        public LifeGameModel(IDataAccess data)
        {
            _dataAccess = data;
            _table = _dataAccess.Load(12);
            timer = new System.Timers.Timer();
            timer.Interval = 1000;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(NextMove);
            IsGoing = false;
            

        }
        public void NewGame()
        {
            _table = _dataAccess.Load(12);
            IsGoing = false;
            

            TableChanged?.Invoke(this, new TableChangedEventArgs(_table));
        }
        public void Set(int x, int y)
        {
           if (!IsGoing)
            {
                _table.set(x, y);
                TableChanged?.Invoke(this, new TableChangedEventArgs(_table));
            }
           
        }
        private void NextMove(Object sender, System.Timers.ElapsedEventArgs e)
        {
            
                _table.NextRound();
                TableChanged?.Invoke(this, new TableChangedEventArgs(_table));
            
        }
        public void StartStop() {
            if (IsGoing) {
                IsGoing = false;
                timer.Stop();
            }
            else
            {
                Console.Error.WriteLine("hello");
                IsGoing = true;
                timer.Start();
            }
        }

    }
}
