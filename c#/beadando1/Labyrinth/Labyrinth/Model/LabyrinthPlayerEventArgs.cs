using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labyrinth.Model
{
    public class LabyrinthPlayerEventArgs : EventArgs
    {
        private List<Tuple<Int32,Int32>> _visibleCoords;
        public List<Tuple<Int32, Int32>> VisibleCoords { get { return _visibleCoords; } }
        public LabyrinthPlayerEventArgs(List<Tuple<Int32, Int32>> coords) { _visibleCoords = coords; }
    }
}
