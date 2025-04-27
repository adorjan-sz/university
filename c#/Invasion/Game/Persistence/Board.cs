using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Game.Persistence
{
    public interface IEntity
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Tuple<int, int> XY { get;  }
        public bool IsEnemy { get;  }
        public bool IsField { get;  }
    }
    public class Field : IEntity
    {
        private int x;
        private int y;
        private bool isEnemy;
        private bool isField;
        public int X { get { return x; } set { x = value; } }
        public int Y { get { return y; } set { y = value; } }
        public Tuple<int,int> XY => new Tuple<int, int>(X,Y);
        public bool IsEnemy { get { return isEnemy; } }
        public bool IsField { get { return isField; } }
        public Field(int x, int y)
        {
            X = x;
            Y = y;
            isEnemy = false;
            isField = true;
        }
    }
    public class Player : IEntity
    {
        private int x;
        private int y;
        private bool isEnemy;
        private bool isField;
        public int X { get { return x; } set { x = value; } }
        public int Y { get { return y; } set { y = value; } }
        public Tuple<int, int> XY => new Tuple<int, int>(X, Y);
        public bool IsEnemy { get { return isEnemy; } }
        public bool IsField { get { return isField; } }
        public Player(int x, int y)
        {
            X = x;
            Y = y;
            isEnemy = false;
            isField = false;
        }
    }
    public class Enemy : IEntity
    {
        private int x;
        private int y;
        private bool isEnemy;
        private bool isField;
        public int X { get { return x; } set { x = value; } }
        public int Y { get { return y; } set { y = value; } }
        public Tuple<int, int> XY => new Tuple<int, int>(X, Y);
        public bool IsEnemy { get { return isEnemy; } }
        public bool IsField { get { return isField; } }
        public Enemy(int x, int y)
        {
            X = x;
            Y = y;
            isEnemy = true;
            isField = false;
        }
    }
    public class Board
    {
        private IEntity[,] _board;
        private Random _random;
        private int health;
        private int _killcount;
        public int Size;
        public int Health => health;
        public IEntity this[int x, int y]
        {
            get => _board[x, y];
            set => _board[x, y] = value;
        }
        public Board(int n)
        {
            _board = new IEntity[n,n];
            _random = new Random();
            health = 10;
            _killcount = 0;
            Size = n;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    _board[i, j] = new Field(i, j);
                }
            }
            int y = _random.Next(n);
            _board[1, y] = new Player(1, y);
            y = _random.Next(n);
            _board[2, y] = new Player(2, y);
            
        }
        public void PlacePlayer(int x, int y)
        {
            if(_killcount>=3)
            {

                _board[x, y] = new Player(x, y);
                _killcount -= 3;
            }
           

        }
        public void GenerateEnemy()
        {
            int x = Size - 1;
            int y = _random.Next(Size);
            _board[x, y] = new Enemy(x, y);
        }
        public void AdvenceEnemy()
        {
            
           for(int i= 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    if (_board[i, j].IsEnemy)
                    {
                        if (i!=0 && _board[i-1, j].IsField)
                        {
                            _board[i - 1, j] = new Enemy(i-1,j);
                            _board[i, j] = new Field(i, j);
                        }
                        else if (i != 0 && !_board[i - 1, j].IsEnemy)
                        {
                            _board[i - 1, j] = new Field(i, j);
                            _board[i, j] = new Field(i, j);
                            
                        }
                    }
                }
            }
            ;
        }
        public void Defend() {
            
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    if (!_board[i, j].IsEnemy && !_board[i,j].IsField)
                    {
                        if(j-1 >= 0 && _board[i, j - 1].IsEnemy)
                        {
                            _killcount++;
                            _board[i, j - 1] = new Field(i, j - 1);
                        }
                        if(j+1 < Size && _board[i, j + 1].IsEnemy)
                        {
                            _killcount++;
                            _board[i, j +1] = new Field(i, j + 1);
                        }
                        
                    }
                }
            }
        }
        public void IsEnemyAtTheEnd()
        {
            for (int i = 0; i < Size; i++)
            {
                if (_board[0, i].IsEnemy)
                {
                    health--;
                    _board[0, i] = new Field(0, i);
                }
            }
            
        }
    }
}
