using System;
using System.IO;
using System.Threading.Tasks;

namespace ELTE.Sudoku.Persistence
{
    public class LabyrinthFileDataAccess : ILabyrinthDataAccess
    {
        public LabyrinthTable LoadEasy()
        {
            string path = @"Persistence\Difficulty\Easy.txt";
            
            try {
                return Load(path);
            }
            catch
            {

                bool[,] matrix = new bool[10, 10]
                    {
                            { true, true, true, true, true, true, true, true, true, false },
                            { true, true, true, true, false, false, false, false, false, false },
                            { true, true, true, true, false, true, true, true, true, true },
                            { true, false, false, false, false, false, false, false, true, true },
                            { true, false, true, true, true, true, true, true, true, false },
                            { true, false, false, false, false, false, false, false, false, false },
                            { true, true, true, true, true, true, true, true, true, false },
                            { true, true, true, true, true, true, true, true, true, false },
                            { true, true, true, false, true, true, true, true, true, false },
                            { false, false, false, false, false, false, false, false, false, false }
                    };
                Int32 size = 10;
                LabyrinthTable table = new LabyrinthTable(size);

                for (Int32 i = 0; i < size; i++)
                {



                    for (Int32 j = 0; j < size; j++)
                    {
                        table.SetValue(i, j, matrix[i, j]);
                    }
                }
                return table;
            }
        }
        public LabyrinthTable LoadMedium()
        {
            string path = @"Persistence\Difficulty\Medium.txt";
            
            try
            {
                return Load(path);
            }
            catch
            {
                bool[,] matrix = new bool[11, 11]
                        {
                        { true, true, true, true, true, true, true, true, true, true, false },
                        { true, false, false, false, false, false, false, false, false, false, false },
                        { true, false, true, true, true, true, true, true, true, true, true },
                        { true, false, true, true, true, true, true, true, true, true, false },
                        { true, false, false, false, false, true, true, true, true, true, false },
                        { true, true, true, true, false, false, false, false, false, false, false },
                        { true, false, false, true, false, true, true, true, true, true, true },
                        { true, false, true, true, false, false, false, false, true, true, true },
                        { true, false, false, false, true, true, true, false, true, true, true },
                        { true, true, true, false, false, false, false, false, true, true, true },
                        { false, false, false, false, true, true, true, true, true, true, true }
                        };
                Int32 size = 11;
                LabyrinthTable table = new LabyrinthTable(size);

                for (Int32 i = 0; i < size; i++)
                {



                    for (Int32 j = 0; j < size; j++)
                    {
                        table.SetValue(i, j, matrix[i, j]);
                    }
                }
                return table;
            }
        }
        public LabyrinthTable LoadHard()
        {
            string path = @"Persistence\Difficulty\Hard.txt";
           
            try { 
                return Load(path); 
            }
            catch {
                bool[,] matrix = new bool[12, 12]
                {
                    { true, true, true, true, true, true, true, true, true, true, true, false },
                    { true, true, true, true, true, true, true, true, true, false, true, false },
                    { true, true, true, true, true, true, true, true, true, false, true, false },
                    { true, true, true, false, false, false, false, false, false, false, true, false },
                    { true, true, true, false, true, true, true, true, true, true, true, false },
                    { true, false, false, false, true, false, false, false, false, false, true, false },
                    { true, false, true, false, false, false, true, true, true, true, true, false },
                    { true, false, true, false, true, false, false, false, false, false, false, false },
                    { true, false, true, false, true, true, true, true, true, true, true, true },
                    { true, false, false, false, false, true, false, false, false, true, true, true },
                    { true, false, true, false, true, true, false, true, false, true, true, true },
                    { false, false, false, false, false, false, false, true, true, true, true, true }
                };
                Int32 size = 12;
                LabyrinthTable table = new LabyrinthTable(size);

                for (Int32 i = 0; i < size; i++)
                {



                    for (Int32 j = 0; j < size; j++)
                    {
                        table.SetValue(i, j, matrix[i, j]);
                    }
                }
                return table;
            }
        }
        public LabyrinthTable Load(String path)
        {
            try
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    String line = reader.ReadLine() ?? String.Empty;
                    Int32 size = Int32.Parse(line);
                    LabyrinthTable table = new LabyrinthTable(size);
                    String[] vals;
                    for (Int32 i = 0; i < size; i++)
                    {
                        line = reader.ReadLine() ?? String.Empty;
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