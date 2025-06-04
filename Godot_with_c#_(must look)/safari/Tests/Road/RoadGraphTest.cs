using Godot;
using GdUnit4;
using System.Collections.Generic;
using Safari.Scripts.Game.Road;
using static GdUnit4.Assertions;
using System.Linq;
using Safari.Scripts.Game.Tiles;
using Safari.Scripts.Game;

namespace Safari.Tests.Road
{
    [TestSuite]
    public class RoadGraphTest
    {
        [TestCase]
        public void GenerateDefaultRoad_AddsConnectedCells()
        {
            var map = new MapData(5, 5, 2);
            var road = new RoadGraph(5, 5);

            for (int x = 0; x < 5; x++)
            {
                var pos = new Vector2I(x, 2);
                map.SetTile(pos, 0, new GrassTile());   // Layer 0 = passable
                map.SetTile(pos, 1, new EmptyTile());   // Layer 1 = no blocking object
            }

            var path = road.GenerateDefaultRoad(map);
            AssertThat(path.Count).IsEqual(5);

            foreach (var cell in path)
            {
                var neighbors = road.GetRoadNeighbors(cell);
                if (cell != road.Exit)
                    AssertThat(neighbors.Count > 0).IsTrue();
            }
        }

        [TestCase]
        public void GetEdgeDirection_CorrectlyMapsVectors()
        {
            var road = new RoadGraph(1, 1);
            AssertThat(road.GetEdgeDirection(new Vector2I(0, 0), new Vector2I(0, 1)).CompareTo('U'));
            AssertThat(road.GetEdgeDirection(new Vector2I(0, 1), new Vector2I(0, 0)).CompareTo('D'));
            AssertThat(road.GetEdgeDirection(new Vector2I(1, 0), new Vector2I(0, 0)).CompareTo('L'));
            AssertThat(road.GetEdgeDirection(new Vector2I(0, 0), new Vector2I(1, 0)).CompareTo('R'));
            // Non-orthogonal
            AssertThat(road.GetEdgeDirection(new Vector2I(0, 0), new Vector2I(1, 1)).CompareTo('?'));
        }

        [TestCase]
        public void GetNextCellTowards_NoRoadOrNoPath_ReturnsSelf()
        {
            var road = new RoadGraph(2, 2);
            var cell = new Vector2I(0, 0);
            var target = new Vector2I(1, 1);
            // No road cells: returns cell
            AssertThat(road.GetNextCellTowards(cell, target)).IsEqual(cell);

            // Add only start node: no path to target
            road.AddRoadCell(cell);
            AssertThat(road.GetNextCellTowards(cell, target)).IsEqual(cell);
        }

        [TestCase]
        public void GetNextCellTowards_PathExists()
        {
            var road = new RoadGraph(3, 1);
            // Create a straight path
            road.AddRoadCell(new Vector2I(0, 0));
            road.AddRoadCell(new Vector2I(1, 0));
            road.AddRoadCell(new Vector2I(2, 0));
            // Next from (0,0) towards (2,0) should be (1,0)
            var next = road.GetNextCellTowards(new Vector2I(0, 0), new Vector2I(2, 0));
            AssertThat(next).IsEqual(new Vector2I(1, 0));
        }

        [TestCase]
        public void GetRandomNextCell_OnAndOffRoad()
        {
            var road = new RoadGraph(3, 1);
            // Build a simple straight road
            road.AddRoadCell(new Vector2I(0, 0));
            road.AddRoadCell(new Vector2I(1, 0));
            road.AddRoadCell(new Vector2I(2, 0));

            // On road: should return a neighbor
            var a = new Vector2I(1, 0);
            var chosen = road.GetRandomNextCell(a);
            AssertThat(chosen == new Vector2I(0, 0) || chosen == new Vector2I(2, 0)).IsTrue();

            // Off road: returns nearest road cell (2,0)
            var off = new Vector2I(5, 5);
            AssertThat(road.GetRandomNextCell(off)).IsEqual(new Vector2I(2, 0));
        }
    }
}