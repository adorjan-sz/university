using Godot;
using GdUnit4;
using Safari.Scripts.Game.Tiles;
using Safari.Scripts.Game;
using static GdUnit4.Assertions;

namespace Safari.Tests
{
    [TestSuite]
    public class MapDataTest
    {
        [TestCase]
        public void SetAndGetTile_ReturnsCorrectTile()
        {
            var map = new MapData(3, 3, 2);
            var tile = new GrassTile();
            var pos = new Vector2I(1, 1);

            map.SetTile(pos, 0, tile);
            AssertThat(map.GetTile(pos, 0)).IsEqual(tile);
        }

        [TestCase]
        public void GetTile_OutOfBounds_ReturnsNull()
        {
            var map = new MapData(2, 2, 1);
            var pos = new Vector2I(-1, 0);

            AssertThat(map.GetTile(pos, 0)).IsNull();
        }

        [TestCase]
        public void IsInBounds_WorksAsExpected()
        {
            var map = new MapData(2, 2, 2);

            AssertThat(map.IsInBounds(new Vector2I(0, 0), 0)).IsTrue();
            AssertThat(map.IsInBounds(new Vector2I(2, 0), 0)).IsFalse();
            AssertThat(map.IsInBounds(new Vector2I(0, 2), 0)).IsFalse();
            AssertThat(map.IsInBounds(new Vector2I(1, 1), 3)).IsFalse();
        }

        [TestCase]
        public void CanPlace_RejectsOverlappingObjects()
        {
            var map = new MapData(2, 2, 2);
            var pos = new Vector2I(1, 1);

            map.SetTile(pos, 0, new GrassTile());
            map.SetTile(pos, 1, new TreeTile(TreeVariant.Full, false));

            var flower = new FlowerTile(false);
            AssertThat(map.CanPlace(pos, flower)).IsFalse();
        }

        [TestCase]
        public void CanPlace_AcceptsValidPlacement()
        {
            var map = new MapData(2, 2, 2);
            var pos = new Vector2I(1, 1);

            map.SetTile(pos, 0, new GrassTile());
            map.SetTile(pos, 1, new EmptyTile());

            var log = new LogTile(false);
            AssertThat(map.CanPlace(pos, log)).IsTrue();
        }

        [TestCase]
        public void CanPlace_RejectsOnWater()
        {
            var map = new MapData(2, 2, 2);
            var pos = new Vector2I(0, 0);

            map.SetTile(pos, 0, new WaterTile());

            var flower = new FlowerTile(false);
            AssertThat(map.CanPlace(pos, flower)).IsFalse();
        }

        [TestCase]
        public void CanPlace_RejectsIfOverlayNotEmpty()
        {
            var map = new MapData(1, 1, 2);
            var pos = new Vector2I(0, 0);

            map.SetTile(pos, 0, new GrassTile());
            map.SetTile(pos, 1, new FlowerTile(false));

            var ground = new DirtTile();
            AssertThat(map.CanPlace(pos, ground)).IsFalse();
        }

        [TestCase]
        public void CanPlace_AcceptsGroundOnEmptyOverlay()
        {
            var map = new MapData(1, 1, 2);
            var pos = new Vector2I(0, 0);

            map.SetTile(pos, 0, new GrassTile());
            map.SetTile(pos, 1, new EmptyTile());

            var dirt = new DirtTile();
            AssertThat(map.CanPlace(pos, dirt)).IsTrue();
        }

        [TestCase]
        public void CanPlace_RejectsTreeOnDirt()
        {
            var map = new MapData(1, 1, 2);
            var pos = new Vector2I(0, 0);

            map.SetTile(pos, 0, new DirtTile());
            map.SetTile(pos, 1, new EmptyTile());

            var tree = new TreeTile(TreeVariant.Full, false);
            AssertThat(map.CanPlace(pos, tree)).IsFalse();
        }

        [TestCase]
        public void CanPlace_AcceptsNonFloraTilesOnAnyBase()
        {
            var map = new MapData(1, 1, 2);
            var pos = new Vector2I(0, 0);

            map.SetTile(pos, 0, new GrassTile());
            map.SetTile(pos, 1, new EmptyTile());

            var genericOverlay = new RoadTile();
            AssertThat(map.CanPlace(pos, genericOverlay)).IsTrue();
        }
    }
}