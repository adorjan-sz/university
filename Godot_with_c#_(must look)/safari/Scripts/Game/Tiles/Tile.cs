using Godot;

namespace Safari.Scripts.Game.Tiles
{
    public abstract partial class Tile : Resource
    {
        public int Layer { get; private set; }
        public int AtlasId { get; protected set; }
        public Vector2I AtlasCoord { get; protected set; }
        public bool IsPassable { get; private set; }
        public bool EmitsLight { get; private set; }

        public void Grow(double delta)
        {
            // Implement growth logic here
        }

        public Tile(int layer, int atlasId, Vector2I atlasCoord, bool isPassable, bool emitsLight)
        {
            Layer = layer;
            AtlasId = atlasId;
            AtlasCoord = atlasCoord;
            IsPassable = isPassable;
            EmitsLight = emitsLight;
        }
    }
}
