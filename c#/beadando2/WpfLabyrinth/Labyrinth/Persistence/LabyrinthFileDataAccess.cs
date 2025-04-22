using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labyrinth.Persistence
{
    public class LabyrinthFileDataAccess : ILabyrinthDataAccess
    {
        public LabyrinthTable LoadEasy()
        {
            string path = @"Persistence\Difficulty\Easy.txt";
            return Load(path);
        }
        public LabyrinthTable LoadMedium()
        {
            string path = @"Persistence\Difficulty\Medium.txt";
            return Load(path);
        }
        public LabyrinthTable LoadHard()
        {
            string path = @"Persistence\Difficulty\Hard.txt";
            return Load(path);
        }
        public LabyrinthTable Load(String path)
        {
            try
            {
                using (StreamReader reader = new StreamReader(path)) 
                {
                    String line =  reader.ReadLine() ?? String.Empty;
                    Int32 size = Int32.Parse(line);
                    LabyrinthTable table = new LabyrinthTable(size); 
                    String[] vals;
                    for (Int32 i = 0; i < size; i++)
                    {
                        line =  reader.ReadLine() ?? String.Empty;
                        vals = line.Split(' ');

                        for (Int32 j = 0; j < size; j++)
                        {
                            table.SetValue(i, j, Boolean.Parse(vals[j]));
                        }
                    }


                    return table;
                }
            }
            catch
            {
                throw new LabyrinthDataException();
            }
        }


    }
}
