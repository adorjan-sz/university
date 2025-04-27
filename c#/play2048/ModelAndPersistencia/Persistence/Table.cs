using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ModelAndPersistencia.Persistence
{
    public class Table
    {   public bool Over { get;set; }
        public int Size { get; set; }
        private int[,] _values;
        private bool[,] _locks;
        public int GetValue(int x,int y)
        {
            return _values[x,y];
        }

        public Table(int size)
        {
            Size = size;
            _values = new int[size,size];
            _locks = new bool[size,size];
            for(int i = 0; i < size; i++)
            {
                for(int j = 0; j < size; j++)
                {
                    _locks[i, j] = false;
                }
            }
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    _values[i, j] =0;
                }
            }
            GenerateFields(2);

        }
        public void MoveRight()
        {
            
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    _locks[i, j] = false;
                }
            }
            
            for (int i = 0; i < Size; i++)
            {
                while (ScanRowForEmpty(i)) {
                    for (int j = Size - 1; j > 0; j--)
                    {
                        if (_values[i, j] == 0 && _values[i, j - 1] != 0)
                        {
                            _values[i, j] = _values[i, j - 1];
                            _values[i, j - 1] = 0;
                            _locks[i, j] = _locks[i, j - 1];
                            _locks[i, j - 1] = false;

                        }
                        else if (_values[i, j] == _values[i, j - 1] && !_locks[i, j] && !_locks[i, j - 1])
                        {
                            _values[i, j] = _values[i, j] * 2;
                            _values[i, j - 1] = 0;
                            _locks[i, j] = true;
                            _locks[i, j - 1] = false;
                        }
                        if(_values[i, j] == 8)
                        {
                            Over = true;
                        }
                    }
                }
                   
            }
            GenerateFields(1);
            

        }
        public void MoveUp()
        {

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    _locks[i, j] = false;
                }
            }

            for (int j = 0; j < Size; j++)
            {

                while (ScanColumnForEmptyUp(j))
                {
                    for (int i = Size - 1; i > 0; i--)
                    {
                        if (_values[i, j] != 0 && _values[i - 1, j] == 0)
                        {

                            _values[i - 1, j] = _values[i, j];

                            _values[i, j] = 0;
                            _locks[i - 1, j] = _locks[i, j];
                            _locks[i, j] = false;

                        }
                        else if (_values[i, j] == _values[i - 1, j] && !_locks[i, j] && !_locks[i - 1, j])
                        {
                            _values[i - 1, j] = _values[i - 1, j] * 2;
                            _values[i, j] = 0;
                            _locks[i - 1, j] = true;
                            _locks[i, j] = false;
                        }
                    }
                }

            }
            GenerateFields(1);


        }
        public void MoveDown()
        {

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    _locks[i, j] = false;
                }
            }

            for (int j = 0; j < Size; j++)
            {

                while (ScanColumnForEmptyDown(j))
                {
                    for (int i = Size - 1; i > 0; i--)
                    {
                        if (_values[i, j] == 0 && _values[i - 1, j] != 0)
                        {

                            

                            _values[i, j] = _values[i - 1, j];
                            _values[i - 1, j] = 0;
                            _locks[i, j] = _locks[i - 1, j];
                            _locks[i - 1, j] = false;

                        }
                        else if (_values[i, j] == _values[i - 1, j] && !_locks[i, j] && !_locks[i - 1, j])
                        {
                            
                            _values[i, j] = _values[i, j]*2;
                            _values[i - 1, j] = 0;
                            _locks[i - 1, j] = false;
                            _locks[i, j] = true;
                        }
                    }
                }

            }
            GenerateFields(1);


        }
        public bool ScanColumnForEmptyUp(int j)
        {

            for (int i = Size - 1; i>0; i--)
            {
                if ((_values[i, j] != 0 && _values[i-1, j] == 0) || (_values[i, j] == _values[i - 1, j] && !_locks[i, j] && !_locks[i - 1, j]))
                {
                    return true;
                }
            }
            return false;
        }
        public bool ScanColumnForEmptyDown(int j)
        {

            for (int i = 0; i < (Size - 1); i++)
            {
                if ((_values[i, j] != 0 && _values[i + 1, j] == 0) || (_values[i, j] == _values[i  +1, j] && !_locks[i, j] && !_locks[i + 1, j]))
                {
                    return true;
                }
            }
            return false;
        }
        public void MoveLeft() {

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    _locks[i, j] = false;
                }
            }

            for (int i = 0; i < Size; i++)
            {
                while (ScanRowForEmptyleft(i))
                {
                    for (int j = 0; j < Size-1; j++)
                    {
                        if (_values[i, j] == 0 && _values[i, j + 1] != 0)
                        {
                            _values[i, j] = _values[i, j + 1];
                            _values[i, j +1] = 0;
                            _locks[i, j] = _locks[i, j + 1];
                            _locks[i, j + 1] = false;

                        }
                        else if (_values[i, j] == _values[i, j + 1] && !_locks[i, j] && !_locks[i, j + 1])
                        {
                            _values[i, j] = _values[i, j] * 2;
                            _values[i, j + 1] = 0;
                            _locks[i, j] = true;
                            _locks[i, j + 1] = false;
                        }
                    }
                }

            }
            GenerateFields(1);

        }
        
     
        public bool ScanRowForEmpty(int i) { 

        for(int j = 0; j < Size-1; j++)
            {
                if ((_values[i,j]!=0 && _values[i, j+1] == 0 )||(_values[i, j] == _values[i, j + 1] && !_locks[i,j] && !_locks[i, j + 1]))
                {
                    return true;
                }
            }
            return false;
        }
        public bool ScanRowForEmptyleft(int i)
        {

            for (int j = Size - 1; j > 0; j--)
            {
                if ((_values[i, j] != 0 && _values[i, j - 1] == 0) || (_values[i, j] == _values[i, j - 1] && !_locks[i, j] && !_locks[i, j - 1]))
                {
                    return true;
                }
            }
            return false;
        }
        
        private void GenerateFields(int count)
        {
            Random random = new Random();

            if (count == 2)
            {
                int x;
                int y;

                do
                {
                    x = random.Next(Size);
                    y = random.Next(Size);
                } while (_values[x, y] != 0);
                switch (random.Next(2))
                {
                    case 0:
                        _values[x, y] = 2;
                        break;
                    case 1:
                        _values[x, y] = 4;
                        break;
                }

                do
                {
                    x = random.Next(Size);
                    y = random.Next(Size);
                } while (_values[x, y] != 0);
                switch (random.Next(2))
                {
                    case 0:
                        _values[x, y] = 2;
                        break;
                    case 1:
                        _values[x, y] = 4;
                        break;
                }
            }
            else if (count == 1)
            {
                int x;
                int y;

                do
                {
                    x = random.Next(Size);
                    y = random.Next(Size);
                } while (_values[x, y] != 0);
                switch (random.Next(5))
                {
                    case 0:
                        _values[x, y] = 4;
                        break;
                    default: _values[x, y] = 2;break;
                }
            }
        }
    }
}
