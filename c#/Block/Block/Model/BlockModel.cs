using Block.Model.persistence;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Block.Model
{
   public enum BlockType
    {
        V,H,L,T,N
    }

    public class BlockModel
    {
        int _score;
        BlockType[,] _table;
        BlockType _current;
        IDataAccess _dataAccess;
        Random Random;
        bool gameOver;
        BlockType[,] Table { get { return _table; } }
        BlockType Current { get { return _current; } }

        public event EventHandler<TableEventArgs>? TableChanged;
        public event EventHandler<CurrentEventArgs>? CurrentChanged;
        public event EventHandler<GameOverEventArgs>? GameOver;
        public event EventHandler<ScoreEventArgs>? ScoreChanged;

        public BlockModel(IDataAccess dataAccess)
        {
            _score = 0;
            _table = new BlockType[4, 4];
            _current = BlockType.N;
            _dataAccess = dataAccess;
            Random = new Random();
            
        }
        public void NewGame()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    _table[i, j] = BlockType.N;
                }
            }
            _score = 0;
            ScoreChanged?.Invoke(this, new ScoreEventArgs(_score));
            gameOver = false;
            NewCurrent();
            TableChanged?.Invoke(this, new TableEventArgs(_table));
        }
        public void NewCurrent()
        {
            _current = (BlockType)Random.Next(4);
            CurrentChanged?.Invoke(this, new CurrentEventArgs(_current));
        }

        public void SetBlock(int x, int y)
        {
            if (gameOver)
            {
                return;
            }
            switch (_current)
            {
                case BlockType.V:
                    try
                    {
                        if (_table[x, y] == BlockType.N && _table[x + 1, y] == BlockType.N)
                        {
                            _table[x, y] = BlockType.V;
                            _table[x + 1, y] = BlockType.V;
                            _score++;
                            ScoreChanged?.Invoke(this, new ScoreEventArgs(_score));
                            NewCurrent();
                            IsThereFull();
                            IsGameOver();
                        }


                    }
                    catch
                    {
                        ;
                    }
                    TableChanged?.Invoke(this, new TableEventArgs(_table));
                    break;

                case BlockType.H:
                    try
                    {
                        if (_table[x, y] == BlockType.N && _table[x, y + 1] == BlockType.N)
                        {
                            _table[x, y] = BlockType.H;
                            _table[x, y + 1] = BlockType.H;
                            _score++;
                            ScoreChanged?.Invoke(this, new ScoreEventArgs(_score));
                            NewCurrent();
                            IsThereFull();
                            IsGameOver();
                        }

                    }
                    catch
                    {
                        ;
                    }
                    TableChanged?.Invoke(this, new TableEventArgs(_table));
                    break;

                case BlockType.L:
                    try
                    {
                        if (_table[x, y] == BlockType.N && _table[x + 1, y + 1] == BlockType.N && _table[x + 1, y] == BlockType.N)
                        {
                            _table[x, y] = BlockType.L;
                            _table[x + 1, y] = BlockType.L;
                            _table[x + 1, y + 1] = BlockType.L;
                            _score++;
                            ScoreChanged?.Invoke(this, new ScoreEventArgs(_score));
                            NewCurrent();
                            IsThereFull();
                            IsGameOver();
                        }

                    }
                    catch
                    {
                        ;
                    }
                    TableChanged?.Invoke(this, new TableEventArgs(_table));
                    break;

                case BlockType.T:
                    try
                    {
                        if (_table[x, y] == BlockType.N && _table[x, y + 1] == BlockType.N && _table[x + 1, y + 1] == BlockType.N)
                        {
                            _table[x, y] = BlockType.T;
                            _table[x, y + 1] = BlockType.T;
                            _table[x + 1, y + 1] = BlockType.T;
                            _score++;
                            ScoreChanged?.Invoke(this, new ScoreEventArgs(_score));
                            NewCurrent();
                            IsThereFull();
                            IsGameOver();
                        }

                    }
                    catch
                    {
                        ;
                    }
                    TableChanged?.Invoke(this, new TableEventArgs(_table));
                    break;

            }

            TableChanged?.Invoke(this, new TableEventArgs(_table));


        }
        public void IsThereFull()
        {
            for (int i = 0; i < 4; i++)
            {
                bool full = true;
                for (int j = 0; j < 4; j++)
                {
                    if (_table[i, j] == BlockType.N)
                    {
                        full = false;
                    }
                }
                if (full)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        _table[i, j] = BlockType.N;
                    }
                    TableChanged?.Invoke(this, new TableEventArgs(_table));
                }
            }
            for (int i = 0; i < 4; i++)
            {
                bool full = true;
                for (int j = 0; j < 4; j++)
                {
                    if (_table[j, i] == BlockType.N)
                    {
                        full = false;
                    }
                }
                if (full)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        _table[j, i] = BlockType.N;
                    }
                    TableChanged?.Invoke(this, new TableEventArgs(_table));
                }
            }

        }
        public void IsGameOver()
        {
            
            
                bool CanContinue = false;
                
                for (int x = 0; x < 4; x++)
                {
                    for (int y = 0; y < 4; y++)
                    {
                        
                        switch(_current)
                        {
                            case BlockType.V:
                            try {
                                if (_table[x, y] == BlockType.N && _table[x + 1, y] == BlockType.N)
                                {
                                    CanContinue = true;

                                }
                            }
                            catch {; }
                                
                            break;
                            case BlockType.H:
                            try {
                                if (_table[x, y] == BlockType.N && _table[x, y + 1] == BlockType.N)
                                {
                                    CanContinue = true;
                                }
                            }
                            catch {; }
                                break;
                            case BlockType.L:
                            try
                            {
                                if (_table[x, y] == BlockType.N && _table[x + 1, y] == BlockType.N && _table[x + 1, y + 1] == BlockType.N)
                                {
                                    CanContinue = true;
                                }
                            }
                            catch {; }
                                break;
                            case BlockType.T:
                            try
                            {
                                if (_table[x, y] == BlockType.N && _table[x, y + 1] == BlockType.N && _table[x + 1, y + 1] == BlockType.N)
                                {
                                    CanContinue = true;
                                }
                            }
                            catch {; }
                            break;
                        }
                    }

                }
                if (!CanContinue)
                {
                    gameOver = true;
                    GameOver?.Invoke(this, new GameOverEventArgs(true));
                }
            
            
        }
        public async Task Save(string path)
        {
           await _dataAccess.SaveAsync(path, _table,_score);
        }
        public async Task Load(string path)
        {
            (_table,_score) = await _dataAccess.LoadAsync(path);
            TableChanged?.Invoke(this, new TableEventArgs(_table));
            ScoreChanged?.Invoke(this, new ScoreEventArgs(_score));
            gameOver = false;
        }
    }
}
