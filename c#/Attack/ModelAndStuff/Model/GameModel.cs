using ModelAndStuff.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAndStuff.Model
{
    public class GameModel
    {
        private IDataAcces _dataAccess;
        private GameBoard? _gameBoard;
        private int _n;
        private int left;
        private int right;
        private int _nextn;

        private bool _isLeftPlayerTurn;
        private int _whichPlayer;
        public event EventHandler<BoardChangedEventArgs>? BoardChanged;
        public event EventHandler<TurnEventArgs>? TurnChanged;
        public int Size { get { return _n; }
            set { _nextn = value; }
        }
        public GameModel(IDataAcces dataAccess)
        {
            _dataAccess = dataAccess;
            
            _nextn = 6;

        }
        public void NewGame()
        {
            _n = _nextn;
            _gameBoard = _dataAccess.Load(_n);
            _isLeftPlayerTurn = false;
            _whichPlayer = 0;
            left = 0;
            right = 0;
            ChangeTurn();
            BoardChanged?.Invoke(this, new BoardChangedEventArgs(_gameBoard));
        }
        private void ChangeTurn()
        {
            if (_isLeftPlayerTurn)
            {

                _isLeftPlayerTurn = false;
                right++;
                if (right> 4)
                {
                    right = 1;
                }
                _whichPlayer = right;

                while (!FindPlayer())
                {
                    right++;
                    if (right > 4)
                    {
                        right = 1;
                    }
                    _whichPlayer = right;
                }
                _whichPlayer = right;
            }
            else {
                _isLeftPlayerTurn = true;
                left++;
                if(left> 4)
                {
                    left = 1;
                }
                _whichPlayer = left;
                
                while (!FindPlayer())
                {
                    left++;
                    if (left > 4)
                    {
                        left = 1;
                    }
                    _whichPlayer = left;
                }
                _whichPlayer = left;
            }
            TurnChanged?.Invoke(this, new TurnEventArgs(_isLeftPlayerTurn,_whichPlayer));
        }
        private bool FindPlayer()
        {
            for (int i = 0; i < _n; i++)
            {
                for (int j = 0; j < _n; j++)
                {
                    if (_gameBoard.GetElement(i, j).IsPlayer &&
                        _gameBoard.GetElement(i, j).IsLeftPlayer == _isLeftPlayerTurn &&
                        _gameBoard.GetElement(i, j).Id == _whichPlayer)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public void Move(int x, int y)
        {
            bool go = true;
            

                for (int i = -1; i < 2; i++)
                {
                    for (int j = -1; j < 2; j++)
                    {
                        if (go)
                        {
                            try
                            {
                                if (_gameBoard.GetElement(x + i, y + j).IsPlayer &&
                                    _gameBoard.GetElement(x + i, y + j).Id == _whichPlayer)
                                {
                                    if (!_gameBoard.GetElement(x, y).IsPlayer)
                                    {
                                        _gameBoard.SetElement(x, y, _gameBoard.GetElement(x + i, y + j));
                                        _gameBoard.SetElement(x + i, y + j, new Field());
                                        go = false;
                                        _gameBoard.RefreshCoord();
                                    }
                                    else if (_gameBoard.GetElement(x, y).IsPlayer
                                        && _gameBoard.GetElement(x, y).IsLeftPlayer != _isLeftPlayerTurn
                                        && (i != 0 && j != 0))
                                    {
                                        _gameBoard.SetElement(x, y, _gameBoard.GetElement(x + i, y + j));
                                        _gameBoard.SetElement(x + i, y + j, new Field());
                                        go = false;
                                        _gameBoard.RefreshCoord();
                                    }
                                }
                            }
                            catch
                            {
                                ;
                            }
                        }
                        
                    
                }
            }
            ChangeTurn();
            BoardChanged?.Invoke(this, new BoardChangedEventArgs(_gameBoard));
        }
    }
}
