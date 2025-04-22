using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Labyrinth.Persistence;

namespace Labyrinth.Model
{
    public enum GameDifficulty { Easy, Medium, Hard }
    public enum Direction { Left,Up, Right,Down}
    public class LabyrinthGameModel
    {
        #region Fields

        private ILabyrinthDataAccess _dataAccess; // adatelérés
        private  LabyrinthTable _table; // játéktábla
        private GameDifficulty _gameDifficulty; // nehézség
        private Int32 _gameTime; // játékidő
        private Player _player;
        private Boolean IsGameOver;

        public Player player { get { return _player; } }
        public GameDifficulty GameDifficulty { get { return _gameDifficulty; } set { _gameDifficulty = value; } }
        public LabyrinthTable Table { get { return _table; } }
        public Int32 GameTime { get { return _gameTime; } }
        public Boolean IsOver { get { return IsGameOver; } }
        #endregion
        #region Constructor
        public LabyrinthGameModel(ILabyrinthDataAccess dataAccess)
        {
            _player = new Player(0, 0);
            _dataAccess = dataAccess;
            _gameDifficulty = GameDifficulty.Easy;
            
            _gameTime = 0;
            IsGameOver = false;

            LoadGame();
           
            
            if (_table is null)
            {
                throw new Exception("Betöltés nem sikerélt");
            }
            _player.X = _table.Size - 1;
            _player.Y = 0;


        }
        #endregion


        #region Events

        /// <summary>
        /// Mező megváltozásának eseménye.
        /// </summary>
        public event EventHandler<LabyrinthPlayerEventArgs>? PlayerMoved;

        /// <summary>
        /// Játék előrehaladásának eseménye.
        /// </summary>
        public event EventHandler<LabyrinthEventArgs>? GameAdvanced;

        /// <summary>
        /// Játék végének eseménye.
        /// </summary>
        public event EventHandler<LabyrinthEventArgs>? GameOver;

        #endregion


        #region Public methods
        public void NewGame()
        {
            _gameTime = 0;
            IsGameOver = false;

            LoadGame();

            
            _player.X = _table.Size - 1;
            _player.Y = 0;

        }
        public void Step(Direction direction)
        {
            
            if (IsGameOver) // ha már vége a játéknak, nem játszhatunk
                return;
            Int32  h,v;
            
            h = 0;
            v = 0;
            switch (direction)
            {
                case Direction.Left:
                    h = 0;
                    v = -1;
                    break;
                case Direction.Up:
                    h = -1;
                    v = 0;
                    break;
                case Direction.Right:
                    h = 0;
                    v= 1;
                    break;
                case Direction.Down:
                    
                    h = 1;
                    v = 0;
                    break;
            }
            if(_player.X+h<0|| _player.X + h>=_table.Size || _player.Y + v <0 || _player.Y + v >= _table.Size)
            {
                return;
            }
            if (_table.IsWall(_player.X + h, _player.Y + v)) 
                return;

            _player.X = _player.X + h;
            _player.Y = _player.Y + v;
            OnPlayerMoved();

            if (_player.X == 0 && _player.Y == _table.Size - 1) // ha vége a játéknak, jelezzük, hogy győztünk
            {
                IsGameOver = true;
                OnGameOver(true);
            }
        }
        public void AdvanceTime()
        {
            if (IsGameOver) // ha már vége, nem folytathatjuk
                return;

            _gameTime++;
            OnGameAdvanced();
        }
        #endregion


        #region Private methods
        public  void LoadGame()
        {
            
            
             if (_dataAccess == null)
                throw new InvalidOperationException("No data access is provided.");
            else{
                switch (_gameDifficulty) // nehézségfüggő beállítása az időnek, illetve a generált mezőknek
            {
                case GameDifficulty.Easy:
                    this._table =  _dataAccess.LoadEasy();
                    break;
                case GameDifficulty.Medium:
                    this._table =  _dataAccess.LoadMedium();
                    break;
                case GameDifficulty.Hard:
                    this._table =  _dataAccess.LoadHard();
                    break;
            }
            }
             


        }
        private bool InTableBounds(Int32 x,Int32 y)
        {
            if (_player.X + x < 0 || _player.X + x >= _table.Size || _player.Y + y < 0 || _player.Y + y >= _table.Size)
            {
                return false;
            }
            return true;
        }
        public void FirsPosition()
        {
            
            OnPlayerMoved();
        }
        #endregion

        private void OnPlayerMoved()
        {
            List<Tuple<Int32,Int32>> res = new List<Tuple<Int32,Int32>>();
            
            //check up
            if (InTableBounds(-1, 0))
            {
                if(InTableBounds(-2, 0) && !_table.IsWall(_player.X - 1, _player.Y))
                {
                    res.Add(Tuple.Create(_player.X -2, _player.Y));
                }
                res.Add(Tuple.Create(_player.X - 1, _player.Y));
            }
            //check up-right corners
            if(InTableBounds(-1, 1))
            {
                if(!_table.IsWall(_player.X - 1, _player.Y) || !_table.IsWall(_player.X, _player.Y + 1))
                {
                    res.Add(Tuple.Create(_player.X - 1, _player.Y+1));
                    if (!_table.IsWall(_player.X-1, _player.Y + 1))
                    {
                        //mivel a sarok látható, ezért a őt érintő mezők is láthatók
                        if (InTableBounds(-2, 1))
                        {
                            res.Add(Tuple.Create(_player.X - 2, _player.Y + 1));
                        }
                        if (InTableBounds(-1, 2))
                        {
                            res.Add(Tuple.Create(_player.X - 1, _player.Y + 2));
                        }
                    }
                }
            }
            //check right
            if (InTableBounds(0, 1))
            {
                if (InTableBounds(0, 2) && !_table.IsWall(_player.X , _player.Y+1))
                {
                    res.Add(Tuple.Create(_player.X, _player.Y+2));
                }
                res.Add(Tuple.Create(_player.X, _player.Y+1));
            }
            //check down-right corners
            if (InTableBounds(1, 1))
            {
                if (!_table.IsWall(_player.X + 1, _player.Y) || !_table.IsWall(_player.X, _player.Y + 1))
                {
                    res.Add(Tuple.Create(_player.X +1, _player.Y + 1));
                    if (!_table.IsWall(_player.X + 1, _player.Y + 1))
                    {
                        //mivel a sarok látható, ezért a őt érintő mezők is láthatók
                        if (InTableBounds(2, 1))
                        {
                            res.Add(Tuple.Create(_player.X + 2, _player.Y + 1));
                        }
                        if (InTableBounds(1, 2))
                        {
                            res.Add(Tuple.Create(_player.X + 1, _player.Y + 2));
                        }
                    }
                }
            }
            //check down
            if (InTableBounds(1, 0))
            {
                if (InTableBounds(2, 0) && !_table.IsWall(_player.X+1, _player.Y))
                {
                    res.Add(Tuple.Create(_player.X+2, _player.Y));
                }
                res.Add(Tuple.Create(_player.X + 1, _player.Y));
            }
            //check left-down
            if (InTableBounds(1, -1))
            {
                if (!_table.IsWall(_player.X + 1, _player.Y) || !_table.IsWall(_player.X, _player.Y - 1))
                {
                    res.Add(Tuple.Create(_player.X + 1, _player.Y - 1));
                    if (!_table.IsWall(_player.X + 1, _player.Y - 1))
                    {
                        //mivel a sarok látható, ezért a őt érintő mezők is láthatók
                        if (InTableBounds(2, -1))
                        {
                            res.Add(Tuple.Create(_player.X + 2, _player.Y - 1));
                        }
                        if (InTableBounds(1, -2))
                        {
                            res.Add(Tuple.Create(_player.X + 1, _player.Y - 2));
                        }
                    }
                }
            }
            //check left
            if (InTableBounds(0, -1))
            {
                if (InTableBounds(0, -2) && !_table.IsWall(_player.X, _player.Y-1))
                {
                    res.Add(Tuple.Create(_player.X, _player.Y-2));
                }
                res.Add(Tuple.Create(_player.X , _player.Y-1));
            }
            //check left-up
            if (InTableBounds(-1, -1))
            {
                if (!_table.IsWall(_player.X - 1, _player.Y) || !_table.IsWall(_player.X, _player.Y - 1))
                {
                    res.Add(Tuple.Create(_player.X - 1, _player.Y - 1));
                    if (!_table.IsWall(_player.X - 1, _player.Y - 1))
                    {
                        //mivel a sarok látható, ezért a őt érintő mezők is láthatók
                        if (InTableBounds(-2, -1))
                        {
                            res.Add(Tuple.Create(_player.X - 2, _player.Y - 1));
                        }
                        if (InTableBounds(-1, -2))
                        {
                            res.Add(Tuple.Create(_player.X - 1, _player.Y - 2));
                        }
                    }
                }
            }
            res.Add(Tuple.Create(_player.X, _player.Y));
            PlayerMoved?.Invoke(this, new LabyrinthPlayerEventArgs(res));
        }

        /// <summary>
        /// Játékidő változás eseményének kiváltása.
        /// </summary>
        private void OnGameAdvanced()
        {
            GameAdvanced?.Invoke(this, new LabyrinthEventArgs(false, _gameTime));
        }

        /// <summary>
        /// Játék vége eseményének kiváltása.
        /// </summary>
        /// <param name="isWon">Győztünk-e a játékban.</param>
        private void OnGameOver(Boolean isWon)
        {
            GameOver?.Invoke(this, new LabyrinthEventArgs(isWon, _gameTime));
        }

    }
}
