using System;
using System.Collections.Generic;

namespace ELTE.Sudoku.Model
{
    /// <summary>
    /// Sudoku mező eseményargumentum típusa.
    /// </summary>

    public class LabyrinthPlayerEventArgs : EventArgs
    {
        private List<Tuple<Int32, Int32>> _visibleCoords;
        public List<Tuple<Int32, Int32>> VisibleCoords { get { return _visibleCoords; } }
        public LabyrinthPlayerEventArgs(List<Tuple<Int32, Int32>> coords) { _visibleCoords = coords; }
    }
}
