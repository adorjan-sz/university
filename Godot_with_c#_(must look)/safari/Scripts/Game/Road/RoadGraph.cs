using System;
using System.Collections.Generic;
using Godot;
using Safari.Scripts.Game.Tiles;
using System.Linq;

namespace Safari.Scripts.Game.Road
{
    /// <summary>
    /// Manages the grid-based road graph: constructs path, maintains cells, and computes an st-orientation.
    /// </summary>
    public class RoadGraph
    {
        public Vector2I Entrance { get; private set; }
        public Vector2I Exit { get; private set; }

        private readonly int _mapWidth;
        private readonly int _mapHeight;

        // Only stores the cells that form the current "road" subgraph
        private readonly List<Vector2I> _cells = new();
        private readonly Dictionary<Vector2I, int> _indexOf = new();
        private readonly List<HashSet<int>> _adj = new();
        private static readonly Random _rng = new Random();

        // Results of st-orientation
        private int[] _stNumber;
        private List<int>[] _directedAdj;

        // 4‑directional moves (up/down/left/right)
        private static readonly Vector2I[] _directions = {
            Vector2I.Up, Vector2I.Down, Vector2I.Left, Vector2I.Right
        };

        /// <summary>
        /// Initialize the graph bounds and compute Entrance/Exit coordinates.
        /// </summary>
        public RoadGraph(int mapWidth, int mapHeight)
        {
            _mapWidth = mapWidth;
            _mapHeight = mapHeight;
            Entrance = new Vector2I(0, _mapHeight / 2);
            Exit = new Vector2I(_mapWidth - 1, _mapHeight / 2);
        }

        /// <summary>
        /// Clears any previous graph data, computes the shortest path
        /// through passable cells, then builds the subgraph and orients edges.
        /// </summary>
        public List<Vector2I> GenerateDefaultRoad(MapData mapData)
        {
            // Reset any existing graph
            _cells.Clear();
            _indexOf.Clear();
            _adj.Clear();

            // Find shortest path purely via BFS
            var path = ComputeShortestPath(mapData);
            if (path.Count == 0)
                return path;

            // Build adjacency only for the path cells
            foreach (var cell in path)
                AddRoadCell(cell);

            // Apply st-orientation to the new subgraph
            RecomputeSTOrientation();
            return path;
        }

        /// <summary>
        /// Perform a BFS on the full grid (base layer + empty overlay),
        /// avoiding non‑passable or occupied tiles, from Entrance to Exit.
        /// </summary>
        private List<Vector2I> ComputeShortestPath(MapData mapData)
        {
            var queue = new Queue<Vector2I>();
            var parent = new Dictionary<Vector2I, Vector2I>();
            var visited = new HashSet<Vector2I>();

            // 1) Initialize BFS
            queue.Enqueue(Entrance);
            visited.Add(Entrance);

            // 2) Expand frontier
            while (queue.Count > 0)
            {
                var cur = queue.Dequeue();
                if (cur == Exit)
                    break;

                foreach (var d in _directions)
                {
                    var nxt = cur + d;
                    // 2.1) Bounds check
                    if (nxt.X < 0 || nxt.X >= _mapWidth ||
                        nxt.Y < 0 || nxt.Y >= _mapHeight)
                        continue;

                    // 2.2) Skip visited
                    if (visited.Contains(nxt))
                        continue;

                    // 2.3) Check terrain and obstacles
                    var baseTile = mapData.GetTile(nxt, 0);
                    var overTile = mapData.GetTile(nxt, 1);
                    if (baseTile == null || !baseTile.IsPassable)
                        continue;
                    if (overTile != null && !(overTile is EmptyTile))
                        continue;

                    visited.Add(nxt);
                    parent[nxt] = cur;
                    queue.Enqueue(nxt);
                }
            }

            // 3) No path found
            if (!parent.ContainsKey(Exit))
                return new List<Vector2I>();

            // 4) Reconstruct path from Exit back to Entrance
            var path = new List<Vector2I>();
            var step = Exit;
            while (step != Entrance)
            {
                path.Add(step);
                step = parent[step];
            }
            path.Add(Entrance);
            path.Reverse();
            return path;
        }

        /// <summary>
        /// Insert a new road cell into the subgraph, linking it
        /// to any adjacent road cells already present.
        /// </summary>
        public void AddRoadCell(Vector2I cell)
        {
            if (_indexOf.ContainsKey(cell))
                return;

            int idx = _cells.Count;
            _cells.Add(cell);
            _indexOf[cell] = idx;
            _adj.Add(new HashSet<int>());

            // Connect to existing neighbors
            foreach (var d in _directions)
            {
                var nbr = cell + d;
                if (_indexOf.TryGetValue(nbr, out var j))
                {
                    _adj[idx].Add(j);
                    _adj[j].Add(idx);
                }
            }

            // Update orientation after mutation
            RecomputeSTOrientation();
        }

        /// <summary>
        /// Removes a road cell from the graph, maintaining a packed array layout
        /// and updating all adjacency links in O(1) time (swap‑delete).
        /// </summary>
        public void RemoveRoadCell(Vector2I cell)
        {
            // 1) Lookup index of the cell; exit if not present
            if (!_indexOf.TryGetValue(cell, out var idx))
                return;

            // 2) Unlink this node from all of its neighbors
            foreach (var nei in _adj[idx])
                _adj[nei].Remove(idx);

            // 3) Swap‑delete: move last cell into idx if needed
            int last = _cells.Count - 1;
            if (idx != last)
            {
                var lastCell = _cells[last];
                _cells[idx] = lastCell;
                _indexOf[lastCell] = idx;
                _adj[idx] = _adj[last];

                // Update all neighbors of moved cell
                foreach (var nbr in _adj[idx])
                {
                    _adj[nbr].Remove(last);
                    _adj[nbr].Add(idx);
                }
            }

            // 4) Remove last slot from all structures
            _cells.RemoveAt(last);
            _adj.RemoveAt(last);
            _indexOf.Remove(cell);

            // 5) Update orientation after mutation
            RecomputeSTOrientation();
        }

        /// <summary>
        /// Delegates st-orientation calculation to RoadGraphStOrienter.
        /// </summary>
        private void RecomputeSTOrientation()
        {
            RoadGraphStOrienter.Orient(
                _cells,
                _indexOf,
                _adj,
                Entrance,
                Exit,
                out _stNumber,
                out _directedAdj);
        }

        /// <summary>
        /// Returns a letter ('U','D','L','R') for the edge from->to.
        /// </summary>
        public char GetEdgeDirection(Vector2I from, Vector2I to)
        {
            var d = to - from;
            if (d == Vector2I.Up) return 'U';
            if (d == Vector2I.Down) return 'D';
            if (d == Vector2I.Left) return 'L';
            if (d == Vector2I.Right) return 'R';
            return '?';
        }

        /// <summary>
        /// Enumerates directed edges after st-orientation.
        /// </summary>
        public IEnumerable<(Vector2I from, Vector2I to)> GetDirectedEdges()
        {
            for (int u = 0; u < _cells.Count; u++)
                foreach (int v in _directedAdj[u])
                    yield return (_cells[u], _cells[v]);
        }

        /// <summary>
        /// Returns the list of directed neighbors (cells you can drive to next)
        /// based on the current st–orientation of the road graph.
        /// </summary>
        /// <param name="cell">The starting cell coordinate.</param>
        /// <returns>A list of adjacent Vector2I road cells; empty if none.</returns>
        public List<Vector2I> GetRoadNeighbors(Vector2I cell)
        {
            if (!_indexOf.TryGetValue(cell, out int u))
                return new List<Vector2I>();

            return _directedAdj[u].Select(v => _cells[v]).ToList();
        }

        /// <summary>
        /// From the given cell, picks a random next road cell. 
        /// If the cell is not itself a road or has no outgoing neighbors,
        /// finds and returns the nearest road cell instead.
        /// </summary>
        /// <param name="cell">The current cell coordinate.</param>
        /// <returns>
        /// A neighboring road cell chosen at random, or if none,
        /// the nearest road cell in the graph.
        /// </returns>
        public Vector2I GetRandomNextCell(Vector2I cell)
        {
            // If this cell isn't part of any road, jump to nearest road
            if (!_indexOf.ContainsKey(cell))
                return GetNearestRoadCell(cell);

            // Otherwise, get true, directed neighbors
            var neighbors = GetRoadNeighbors(cell);
            if (neighbors.Count > 0)
                return neighbors[_rng.Next(neighbors.Count)];

            // Fallback if dead‑end: jump to nearest road
            return GetNearestRoadCell(cell);
        }

        /// <summary>
        /// Finds the closest cell, by BFS distance, that is currently part 
        /// of the road graph. If the starting cell is itself a road, returns it.
        /// </summary>
        /// <param name="from">The coordinate from which to begin the search.</param>
        /// <returns>
        /// The coordinate of the nearest road cell; if none exist, returns 'from'.
        /// </returns>
        public Vector2I GetNearestRoadCell(Vector2I from)
        {
            // If 'from' is already a road, use it
            if (_indexOf.ContainsKey(from))
                return from;

            var queue = new Queue<Vector2I>();
            var visited = new HashSet<Vector2I>();
            queue.Enqueue(from);
            visited.Add(from);

            while (queue.Count > 0)
            {
                var cur = queue.Dequeue();

                // Explore all 4‑directions (undirected grid adjacency)
                foreach (var d in _directions)
                {
                    var nxt = cur + d;
                    if (visited.Add(nxt))
                    {
                        if (_indexOf.ContainsKey(nxt))
                            return nxt;

                        queue.Enqueue(nxt);
                    }
                }
            }

            // No road found anywhere: just return the original cell
            return from;
        }

        /// <summary>
        /// Returns the next cell on the shortest path from <paramref name="cell"/> to 
        /// <paramref name="target"/> using the undirected road adjacency. If no path exists
        /// or either endpoint is not part of the road graph, returns <paramref name="cell"/>.
        /// </summary>
        /// <param name="cell">Current position.</param>
        /// <param name="target">Destination.</param>
        /// <returns>The neighbor cell one step closer to <paramref name="target"/>.</returns>
        public Vector2I GetNextCellTowards(Vector2I cell, Vector2I target)
        {
            // Both start and target must be on the road graph
            if (!_indexOf.TryGetValue(cell, out int startIdx) ||
                !_indexOf.TryGetValue(target, out int targetIdx))
            {
                return cell;
            }

            // BFS setup
            var queue = new Queue<int>();
            var parent = new Dictionary<int, int>();
            var visited = new HashSet<int>();

            queue.Enqueue(startIdx);
            visited.Add(startIdx);

            // Perform BFS until we find the target or exhaust the graph
            while (queue.Count > 0)
            {
                int u = queue.Dequeue();
                if (u == targetIdx)
                    break;

                foreach (int v in _adj[u])
                {
                    if (visited.Add(v))
                    {
                        parent[v] = u;
                        queue.Enqueue(v);
                    }
                }
            }

            // If we never reached the target, stay put
            if (!parent.ContainsKey(targetIdx))
                return cell;

            // Reconstruct the path from start -> target
            var path = new List<int>();
            int step = targetIdx;
            while (step != startIdx)
            {
                path.Add(step);
                step = parent[step];
            }
            path.Add(startIdx);
            path.Reverse();

            // The second element is the next step
            int nextIdx = path.Count >= 2 ? path[1] : startIdx;
            return _cells[nextIdx];
        }
    }
}
