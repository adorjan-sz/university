using System;
using System.IO;
using System.Threading.Tasks;

namespace ELTE.Sudoku.Persistence
{
    /// <summary>
    /// Sudoku fájl kezelő felülete.
    /// </summary>
    public interface ILabyrinthDataAccess
    {
        LabyrinthTable Load(String path);

        LabyrinthTable LoadEasy();
        LabyrinthTable LoadMedium();
        LabyrinthTable LoadHard();


    }
}
