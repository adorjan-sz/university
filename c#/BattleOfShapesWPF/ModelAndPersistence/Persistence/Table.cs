using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAndPersistence.Persistence
{
    public interface IEntity
    {
        public bool IsEmpty { get; init; }
        public bool IsPlayer1 { get; init; }
    }
    public class Player1 : IEntity
    {
        public bool IsEmpty { get; init; }
        public bool IsPlayer1 { get; init; }
        public Player1() {
            IsEmpty = false;
            IsPlayer1 = true;
        }
    }
    public class Player2 : IEntity
    {
        public bool IsEmpty { get; init; }
        public bool IsPlayer1 { get; init; }
        public Player2()
        {
            IsEmpty = false;
            IsPlayer1 = false;
        }
    }
    public class Empty : IEntity
    {
        public bool IsEmpty { get; init; }
        public bool IsPlayer1 { get; init; }
        public Empty()
        {
            IsEmpty = true;
            IsPlayer1 = false;
        }
    }
    public class Table
    {
        private IEntity[,] _table;
        private IEntity[,] _next;
        public IEntity[,] GetTable() { return _table; }
        public IEntity[,] GetNext() { return _next; }
        public int Size { get; init; }
        private Random random;


        public Table(int n)
        {
            Size = n;
            random = new Random();
            _table = new IEntity[Size,Size];
            _next = new IEntity[3,3];
            for(int i = 0; i < Size; i++)
            {
                for(int j = 0;  j < Size; j++)
                {
                    _table[i,j]= new Empty();
                }
            }
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    _next[i, j] = new Empty();
                   
                }
            }

        }
        private bool NextIsEmpty()
        {
            bool r = true;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (!_next[i, j].IsEmpty == true)
                    {
                        r = true;
                    }
                }
            }
            return r;
        }
        public (bool,int) SetNext(int x, int y,bool IsPlayer1Comes) {
            int count = 0;
            if (x > 0 && y > 0 && x < Size - 1 && y < Size - 1)
            {
                for(int i = -1;i < 2; i++)
                {
                    for(int j=-1; j < 2; j++)
                    {
                        if(_next[1+i, 1+j].IsEmpty != true)
                        {
                            if (_table[x + i, y + j].IsEmpty)
                                count++;
                            else if (IsPlayer1Comes && !_table[x + i, y + j].IsPlayer1)
                                count += 2;
                            else if (!IsPlayer1Comes && _table[x + i, y + j].IsPlayer1)
                                count += 2;
                            _table[x + i, y + j] = _next[1 + i, 1 + j];
                        }
                        
                    }
                }
                return (true,count);
                
            }
            else { 
            return (false,count);
            }

        }


        public void CreateNext(bool IsPlayer1Comes)
        {
            int x;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    _next[i, j] = new Empty();

                }
            }
            
            
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        x = random.Next(2);
                        switch (x)
                        {
                            case 0:
                                _next[i, j] = new Empty();
                                break;
                            case 1:
                                if (IsPlayer1Comes)
                                {
                                    _next[i, j] = new Player1();
                                }
                                else
                                {
                                    _next[i, j] = new Player2();
                                }
                                break;
                        }
                    }
                }
            

        }

    }
}
