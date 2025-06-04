using Godot;

namespace Safari.Scripts.Game.Tiles
{
    public partial class RoadTile : Tile, Buyable
    {
        private bool _hasNW;
        public bool HasNW
        {
            get => _hasNW;
            set { _hasNW = value; UpdateAtlasCoordinate(); }
        }

        private bool _hasNE;
        public bool HasNE
        {
            get => _hasNE;
            set { _hasNE = value; UpdateAtlasCoordinate(); }
        }

        private bool _hasSW;
        public bool HasSW
        {
            get => _hasSW;
            set { _hasSW = value; UpdateAtlasCoordinate(); }
        }

        private bool _hasSE;
        public bool HasSE
        {
            get => _hasSE;
            set { _hasSE = value; UpdateAtlasCoordinate(); }
        }

        private int _price;
        int Buyable.Price { get { return _price; } }

        public RoadTile() : base(0, 0, new Vector2I(0, 0), true, false)
        {
            _price = 20;
            _hasNW = false;
            _hasNE = false;
            _hasSW = false;
            _hasSE = false;
            UpdateAtlasCoordinate();
        }

        public void SetNeighbors(bool nw, bool ne, bool sw, bool se)
        {
            _hasNW = nw;
            _hasNE = ne;
            _hasSW = sw;
            _hasSE = se;
            UpdateAtlasCoordinate();
        }

        private void UpdateAtlasCoordinate()
        {
            int index = (_hasNW ? 8 : 0) + (_hasNE ? 4 : 0) + (_hasSW ? 2 : 0) + (_hasSE ? 1 : 0);
            AtlasCoord = new Vector2I(index % 4, index / 4);
        }
    }
}
