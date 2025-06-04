using Godot;
using GdUnit4;
using System.Collections.Generic;
using Safari.Scripts.Game.Road;
using static GdUnit4.Assertions;

namespace Safari.Tests.Road
{
    [TestSuite]
    public class RoadGraphStOrienterTest
    {
        [TestCase]
        public void BasicLinearRoad()
        {
            // Test a simple chain A-B-C
            var cells = new List<Vector2I> { new(0, 0), new(1, 0), new(2, 0) };
            var indexOf = new Dictionary<Vector2I, int>
            {
                [cells[0]] = 0,
                [cells[1]] = 1,
                [cells[2]] = 2
            };
            var adj = new List<HashSet<int>>
            {
                new() { 1 },       // A
                new() { 0, 2 },    // B
                new() { 1 }        // C
            };

            RoadGraphStOrienter.Orient(cells, indexOf, adj, cells[0], cells[2], out var stNumber, out var directedAdj);

            AssertThat(stNumber[0]).IsEqual(1);
            AssertThat(stNumber[1]).IsEqual(2);
            AssertThat(stNumber[2]).IsEqual(3);
            AssertThat(directedAdj[0]).ContainsExactly(1);
            AssertThat(directedAdj[1]).ContainsExactly(2);
            AssertThat(directedAdj[2]).IsEmpty();
        }

        [TestCase]
        public void DisconnectedGraph()
        {
            // Entrance and exit are in different components
            var cells = new List<Vector2I> { new(0, 0), new(1, 0), new(3, 0) };
            var indexOf = new Dictionary<Vector2I, int>
            {
                [cells[0]] = 0,
                [cells[1]] = 1,
                [cells[2]] = 2
            };
            var adj = new List<HashSet<int>>
            {
                new() { 1 },    // A
                new() { 0 },    // B
                new() { }       // C (disconnected)
            };

            RoadGraphStOrienter.Orient(cells, indexOf, adj, cells[0], cells[2], out var stNumber, out var directedAdj);

            foreach (int val in stNumber)
                AssertThat(val).IsEqual(0);
            foreach (var list in directedAdj)
                AssertThat(list).IsEmpty();
        }

        [TestCase]
        public void CycleGraph()
        {
            // A cycle A-B-C-D
            var cells = new List<Vector2I>
            {
                new(0, 0), // A
                new(1, 0), // B
                new(1, 1), // C
                new(0, 1)  // D
            };
            var indexOf = new Dictionary<Vector2I, int>
            {
                [cells[0]] = 0,
                [cells[1]] = 1,
                [cells[2]] = 2,
                [cells[3]] = 3
            };
            var adj = new List<HashSet<int>>
            {
                new() { 1, 3 }, // A
                new() { 0, 2 }, // B
                new() { 1, 3 }, // C
                new() { 0, 2 }  // D
            };

            RoadGraphStOrienter.Orient(cells, indexOf, adj, cells[0], cells[2], out var stNumber, out var directedAdj);

            // Exit (C) should have the highest number
            AssertThat(stNumber[2]).IsEqual(cells.Count);
            // All directed edges must go from lower to higher st-number
            for (int u = 0; u < cells.Count; u++)
            {
                foreach (var v in directedAdj[u])
                {
                    AssertThat(stNumber[u] < stNumber[v]).IsTrue();
                }
            }
        }

        [TestCase]
        public void SingleNodeGraph()
        {
            // Graph with a single vertex (entrance == exit)
            var cells = new List<Vector2I> { new(5, 5) };
            var indexOf = new Dictionary<Vector2I, int> { [cells[0]] = 0 };
            var adj = new List<HashSet<int>> { new() };

            RoadGraphStOrienter.Orient(cells, indexOf, adj, cells[0], cells[0], out var stNumber, out var directedAdj);

            // Only one vertex: should be assigned stNumber = 1 (n=1)
            AssertThat(stNumber[0]).IsEqual(1);
            AssertThat(directedAdj[0]).IsEmpty();
        }

        [TestCase]
        public void TwoNodeGraph_Connected()
        {
            // Two vertices directly connected
            var cells = new List<Vector2I> { new(0, 1), new(1, 1) };
            var indexOf = new Dictionary<Vector2I, int>
            {
                [cells[0]] = 0,
                [cells[1]] = 1
            };
            var adj = new List<HashSet<int>>
            {
                new() { 1 },
                new() { 0 }
            };

            RoadGraphStOrienter.Orient(cells, indexOf, adj, cells[0], cells[1], out var stNumber, out var directedAdj);

            AssertThat(stNumber[0]).IsEqual(1);
            AssertThat(stNumber[1]).IsEqual(2);
            AssertThat(directedAdj[0]).ContainsExactly(1);
            AssertThat(directedAdj[1]).IsEmpty();
        }

        [TestCase]
        public void TwoNodeGraph_Disconnected()
        {
            // Two vertices not connected
            var cells = new List<Vector2I> { new(2, 2), new(3, 3) };
            var indexOf = new Dictionary<Vector2I, int>
            {
                [cells[0]] = 0,
                [cells[1]] = 1
            };
            var adj = new List<HashSet<int>>
            {
                new(),
                new()
            };

            RoadGraphStOrienter.Orient(cells, indexOf, adj, cells[0], cells[1], out var stNumber, out var directedAdj);

            foreach (var val in stNumber)
                AssertThat(val).IsEqual(0);
            foreach (var list in directedAdj)
                AssertThat(list).IsEmpty();
        }

        [TestCase]
        public void MissingEntranceOrExit()
        {
            // indexOf missing exit vertex
            var cells = new List<Vector2I> { new(0, 0), new(1, 0) };
            var indexOf = new Dictionary<Vector2I, int> { [cells[0]] = 0 };
            var adj = new List<HashSet<int>> { new() { 1 }, new() { 0 } };

            RoadGraphStOrienter.Orient(cells, indexOf, adj, cells[0], cells[1], out var stNumber, out var directedAdj);

            // No valid orientation: all zeros
            foreach (var val in stNumber)
                AssertThat(val).IsEqual(0);
            foreach (var list in directedAdj)
                AssertThat(list).IsEmpty();
        }

        [TestCase]
        public void EntranceEqualsExit()
        {
            // Entrance and exit are the same but graph has extra nodes
            var cells = new List<Vector2I> { new(0, 0), new(1, 0) };
            var indexOf = new Dictionary<Vector2I, int>
            {
                [cells[0]] = 0,
                [cells[1]] = 1
            };
            var adj = new List<HashSet<int>>
            {
                new() { 1 },
                new() { 0 }
            };

            RoadGraphStOrienter.Orient(cells, indexOf, adj, cells[0], cells[0], out var stNumber, out var directedAdj);

            // Entrance==exit receives highest stNumber (n = 2), others zero
            AssertThat(stNumber[0]).IsEqual(2);
            AssertThat(stNumber[1]).IsEqual(0);
            AssertThat(directedAdj[0]).IsEmpty();
            AssertThat(directedAdj[1]).IsEmpty();
        }

        [TestCase]
        public void ComplexGraphMultipleBCCs()
        {
            // Complex graph with a cut component and multiple BCCs
            var cells = new List<Vector2I>
            {
                new(0, 0), // 0 entrance
                new(1, 0), // 1
                new(2, 0), // 2
                new(1, 1), // 3
                new(2, 1), // 4 exit
                new(-1, 1),// 5 disconnected
                new(-2, 1) // 6 disconnected
            };
            var indexOf = new Dictionary<Vector2I, int>();

            for (int i = 0; i < cells.Count; i++) 
                indexOf[cells[i]] = i;

            var adj = new List<HashSet<int>>
            {
                new() {1},
                new() {0,2,3},
                new() {1,4},
                new() {1,4},
                new() {2,3},
                new() {6},
                new() {5}
            };

            RoadGraphStOrienter.Orient(cells, indexOf, adj, cells[0], cells[4], out var stNumber, out var directedAdj);

            // Vertices 0-4 should have positive stNumbers
            for (int i = 0; i <= 4; i++)
                AssertThat(stNumber[i] > 0).IsTrue();

            // Disconnected vertices 5-6 should remain zero
            AssertThat(stNumber[5]).IsEqual(0);
            AssertThat(stNumber[6]).IsEqual(0);

            // Exit should get n=7
            AssertThat(stNumber[4]).IsEqual(cells.Count);
            // Directed edges respect st-order
            for (int u = 0; u < cells.Count; u++)
                foreach (var v in directedAdj[u])
                    AssertThat(stNumber[u] < stNumber[v]).IsTrue();
        }
    }
}