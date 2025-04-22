using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantVsZombies.Persistence
{
    public class Board
    {
        private string[,] owners;

        public string[,] Owners
        {
            get { return owners; }
            set { owners = value; }
        }

        public Board()
        {
            owners = new string[5, 10];
        }
    }
}
