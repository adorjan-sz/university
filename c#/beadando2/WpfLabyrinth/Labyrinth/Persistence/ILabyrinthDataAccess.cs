﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labyrinth.Persistence
{
    public interface ILabyrinthDataAccess
    {
        LabyrinthTable Load(String path);

        LabyrinthTable LoadEasy();
        LabyrinthTable LoadMedium();
        LabyrinthTable LoadHard();


    }
}
