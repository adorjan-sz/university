using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Safari.Scripts.Game.Tiles
{
    public partial class EmptyTile : Tile
    {
        public EmptyTile() : base(-1, -1, new Vector2I(-1, -1), true, false)
        {
        }
    }
}
