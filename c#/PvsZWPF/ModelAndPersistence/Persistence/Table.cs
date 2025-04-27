using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAndPersistence.Persistence
{
    public interface IEntity
    {
        public bool IsZombie { get; init; }
        public bool IsPlant { get; init; }
        public bool IsEmpty { get; init; }
    }
    public class Zombie : IEntity
    {
        public bool IsEmpty { get; init; } = false;
        public bool IsPlant { get; init; } = false;
        public bool IsZombie { get; init; } = true;
    }
    public class Plant : IEntity
    {
        public bool IsEmpty { get; init; } = false;
        public bool IsPlant { get; init; } = true;
        public bool IsZombie { get; init; } = false;
    }
    public class Field : IEntity
    {
        public bool IsEmpty { get; init; } = true;
        public bool IsPlant { get; init; } = false;
        public bool IsZombie { get; init; } = false;
    }

    public class Table
    {

        private IEntity[,] _table;
        public int Row { get; set; }
        public int Column { get; set; }
        public IEntity GetEntity(int x,int y)
        {
            return _table[x,y];
        }
        public Table(int row,int col) {
            Row = row;
            Column = col;
            _table = new IEntity[row,col];
            for (int i = 0; i < Row; i++)
            {
                for (int j = 0; j < Column; j++)
                {
                    _table[i,j] = new Field();
                }
            }
        }
        public bool set(int row, int column, IEntity entity)
        {
            if (_table[row, column].IsEmpty)
            {
                _table[row, column] = entity;
                return true;    
            }
            return false;
        }
        public void ShootZombie()
        {
            int dy;
            for (int i = 0; i < Row; i++)
            {
                for (int j = 0; j < Column; j++)
                {
                    if (_table[i, j].IsPlant)
                    {
                        dy = 0;
                        while(j+dy<Column && !_table[i, j + dy].IsZombie)
                        {
                            dy++;
                        }
                        if(dy+j == Column)
                        {
                            ;
                        }
                        else
                        {
                            _table[i, j + dy] = new Field();

                        }
                    }
                }
            }
        }
        public void AdvenceZombie()
        {
            for(int i =0; i < Row; i++)
            {
                for(int j = 0;j<Column;j++)
                {
                    if(_table[i, j].IsZombie)
                    {
                        if(_table[i, j-1].IsEmpty)
                        {
                            _table[i, j - 1] = new Zombie();
                            _table[i, j] = new Field();
                        }else if(_table[i, j - 1].IsPlant)
                        {
                            _table[i, j - 1] = new Field();

                        }
                    }
                }
            }
        }
        public bool ZombieInTheHoouse() {
            for(int i = 0; i < Row; i++)
            {
                if (_table[i, 0].IsZombie)
                {
                    return true;
                }

            }
            return false;
        }
    }
}
