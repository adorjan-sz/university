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
        public bool IsPlayer1Comes {  get; private set; }
        private Table _table;
        private GameDifficulty _gameDifficulty; // nehézség
        private IDataAccess Data;
        public int Player1Count { get; private set; }
        private bool Over;
        public int Player2Count { get; private set; }
        private int End;

        public int TableSize { get; private set; }
        public GameDifficulty GameDifficulty
        {
            get { return _gameDifficulty; }
            set { _gameDifficulty = value; }
        }
        
        public event EventHandler<TableEventArgs> TableChanged;
        public event EventHandler<NextTableEventArgs> NextTableChanged;
        public event EventHandler GameOver;
        public event EventHandler<CountChangedEventArgs> CountChanged ;
        public GameModel(IDataAccess data)
        {
            this.Data = data;
           
            _gameDifficulty = GameDifficulty.Easy;
            _table = Data.EasyLoad();
            TableChanged?.Invoke(this, new TableEventArgs(_table.GetTable()));
        }
        public void NewGame()
        {
            IsPlayer1Comes = true;
            Player1Count = 0;
            Over = false;
            Player2Count = 0;
            switch (_gameDifficulty) // nehézségfüggő beállítása az időnek, illetve a generált mezőknek
            {
                case GameDifficulty.Easy:
                    _table = Data.EasyLoad();
                    break;
                case GameDifficulty.Medium:
                    _table = Data.MediumLoad();
                    break;
                case GameDifficulty.Hard:
                    _table = Data.HardLoad();
                    break;
            }
            TableSize = _table.Size;
            End = TableSize * 2;
            TableChanged?.Invoke(this, new TableEventArgs(_table.GetTable()));
            _table.CreateNext(IsPlayer1Comes);
            NextTableChanged?.Invoke(this,new NextTableEventArgs(_table.GetNext()));
            CountChanged.Invoke(this, new CountChangedEventArgs(Player1Count, Player2Count));

        }
        public void Set(int x, int y)
        {
            if (!Over)
            {
                int val;
                bool worked;
                (worked, val) = _table.SetNext(x, y, IsPlayer1Comes);
                if (worked)
                {
                    if (IsPlayer1Comes)
                    {
                        Player1Count += val;
                    }
                    else
                    {
                        Player2Count += val;
                    }
                    IsPlayer1Comes = !IsPlayer1Comes;
                    TableChanged?.Invoke(this, new TableEventArgs(_table.GetTable()));
                    _table.CreateNext(IsPlayer1Comes);
                    NextTableChanged?.Invoke(this, new NextTableEventArgs(_table.GetNext()));
                    CountChanged.Invoke(this, new CountChangedEventArgs(Player1Count, Player2Count));
                    End--;
                    if (End == 0)
                    {
                        GameOver.Invoke(this, EventArgs.Empty);
                        Over = true;
                    }
                }
            }
            
        }
    }
}
