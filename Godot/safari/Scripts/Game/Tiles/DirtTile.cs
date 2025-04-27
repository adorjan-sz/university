using Godot;
using System.Collections.Generic;

namespace Safari.Scripts.Game.Tiles
{
    public partial class DirtTile : RandomTextureTile
    {
        public DirtTile() : base(0, 1, new List<Vector2I>
            {
                new Vector2I(9, 0),
                new Vector2I(8, 0)
            }, true, false)
        { 

        }
    }
}
