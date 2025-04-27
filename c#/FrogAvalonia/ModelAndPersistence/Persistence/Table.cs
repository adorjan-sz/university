using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace ModelAndPersistence.Persistence
{
    public interface IEntity
    {
        public bool IsEmpty { get; set; }
        public bool IsFrog { get; set; }
        public bool IsCar { get; set; }
        public bool IsSafety { get; set; }
    }
    public class Empty : IEntity {
        public bool IsEmpty { get; set; }
        public bool IsFrog { get; set; }
        public bool IsCar { get; set; }
        public bool IsSafety { get; set; }
        public Empty() {
            IsEmpty = true;
            IsFrog = false;
            IsCar = false;
            IsSafety = false;
        }
    }
    
    public class Safety : IEntity
    {
        public bool IsEmpty { get; set; }
        public bool IsFrog { get; set; }
        public bool IsCar { get; set; }
        public bool IsSafety { get; set; }
        public Safety()
        {
            IsEmpty = false;
            IsFrog = false;
            IsCar = false;
            IsSafety = true;
        }
    }
    public class Car : IEntity
    {
        public bool IsEmpty { get; set; }
        public bool IsFrog { get; set; }
        public bool IsCar { get; set; }
        public bool IsSafety { get; set; }
        public Car()
        {
            IsEmpty = false;
            IsFrog = false;
            IsCar = true;
            IsSafety = false;
        }
    }
    public class Table
    {
        public IEntity[,] table;
        public IEntity this[int x,int y] { get { return table[x,y]; } }
        public int Size { get; set; }
        private int diff;
        private Random random;
        public Table(int n) {
            Size = n;
            switch (Size)
            {
                case 20:
                    diff = 5; break;
                case 16: diff = 4; break;
                case 12: diff = 3; break;
            }
            table = new IEntity[9, n];
            random = new Random();
            for (int i = 0; i < 9; i++)
            {
                for(int j= 0; j < n; j++)
                {
                    table[i,j] = new Empty();
                }
            }
            for(int i = 0;i<n; i++)
            {
                table[0,i] = new Safety();
            }
            for (int i = 0; i < n; i++)
            {
                table[8, i] = new Safety();
            }
            for (int i = 0; i < n; i++)
            {
                table[4, i] = new Safety();
            }
            CreateCarLeftToRight();
            Advence();

        }
        public void Advence() {
            for(int i = 1; i < 4; i++)
            {
                for(int j= 0;j<Size-1; j++)
                {
                    if(table[i,j] is Car)
                    {
                        table[i, j] = new Empty();
                        table[i, j+1] = new Car();
                        j++;
                    }
                }
            }
            CreateCarLeftToRight();
            for (int i = 5; i < 8; i++)
            {
                for (int j = Size-1; j >0; j--)
                {
                    if (table[i, j] is Car)
                    {
                        table[i, j] = new Empty();
                        table[i, j - 1] = new Car();
                        j--;
                    }
                }
            }
            CreateCarRightToLeft();
            for (int i = 1; i < 4; i++)
            {
                
                    if (table[i, Size-1] is Car)
                    {
                        table[i, Size-1] = new Empty();
                        
                    }
                
            }
            for (int i = 5; i < 8; i++)
            {
                
                    if (table[i, 0] is Car)
                    {
                     
                        table[i, 0] = new Empty();
                       
                    }
                
            }

        }
        private void CreateCarRightToLeft()
        {
            int r;
            bool valid = false;


            r = random.Next(3);
            switch (r)
            {
                case 0:
                    int i = Size-1;
                    int count = 0;
                    while (i >0 && !(table[5, i] is Car a))
                    {
                        i--;
                        count++;
                    }
                    if (count > diff)
                        valid = true;

                    break;
                case 1:
                     i = Size - 1;
                    count = 0;
                    while (i > 0 && !(table[6, i] is Car a))
                    {
                        i--;
                        count++;
                    }
                    if (count > diff)
                        valid = true;

                    break;
                case 2:
                    i = Size - 1;
                    count = 0;
                    while (i > 0 && !(table[7, i] is Car a))
                    {
                        i--;
                        count++;
                    }
                    if (count > diff)
                        valid = true;

                    break;

            }
            if (valid)
                table[r + 5, Size-1] = new Car();
        }
        private void CreateCarLeftToRight()
        {
            int r;
            bool valid = false;
            
            
                r = random.Next(3);
                switch (r) {
                    case 0:
                        int i = 0;
                        while ( i < Size && !(table[1, i] is Car a))
                        {
                            i++;
                        }
                        if(i>diff)
                            valid = true;

                        break;
                    case 1:
                         i = 0;
                        while (i < Size && !(table[2, i] is Car a))
                        {
                            i++;
                        }
                        if (i > diff)
                            valid = true;
                        break;
                    case 2:
                         i = 0;
                        while (i < Size && !(table[3, i] is Car a))
                        {
                            i++;
                        }
                        if (i > diff)
                            valid = true;
                        break;
                
            }
            if(valid)
                table[r + 1, 0] = new Car();
        }


    }
}
