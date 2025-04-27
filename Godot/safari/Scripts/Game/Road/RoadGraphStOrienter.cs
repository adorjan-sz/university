using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace Safari.Scripts.Game.Road
{
    /// <summary>
    /// Helper class for computing an st-orientation on a road graph.
    /// Assigns every node an st-number and orients edges so that there is exactly one source (entrance) and one sink (exit).
    /// </summary>
    public static class RoadGraphStOrienter
    {
        /// <summary>
        /// Performs the st-orientation on the given undirected road graph.
        /// </summary>
        /// <param name="cells">List of unique cell coordinates representing vertices.</param>
        /// <param name="indexOf">Dictionary mapping each coordinate to its index in 'cells'.</param>
        /// <param name="adj">Undirected adjacency: for each index, the set of neighbor indices.</param>
        /// <param name="entrance">Coordinate of the unique source vertex.</param>
        /// <param name="exit">Coordinate of the unique sink vertex.</param>
        /// <param name="stNumber">Output: st-number for each vertex after orientation.</param>
        /// <param name="directedAdj">Output: oriented adjacency lists for directed edges.</param>
        public static void Orient(
            List<Vector2I> cells,
            Dictionary<Vector2I, int> indexOf,
            List<HashSet<int>> adj,
            Vector2I entrance,
            Vector2I exit,
            out int[] stNumber,
            out List<int>[] directedAdj)
        {
            int n = cells.Count;
            // Initialize outputs: zero-initialized numbering, empty directed lists
            stNumber = new int[n];
            directedAdj = new List<int>[n];
            for (int i = 0; i < n; i++)
                directedAdj[i] = new List<int>();

            // 1) Connectivity check from entrance: prune unreachable vertices
            var reachable = ComputeReachable(cells, indexOf, adj, entrance);
            if (!indexOf.TryGetValue(exit, out var exitIdx) || !reachable[exitIdx])
            {
                return; // No valid orientation if sink is not reachable
            }

            // 2) Build a working subgraph on reachable vertices only
            var workVerts = new HashSet<int>(
                Enumerable.Range(0, n).Where(u => reachable[u])
            );
            var workAdj = workVerts.ToDictionary(
                u => u,
                u => new HashSet<int>(adj[u].Where(v => reachable[v]))
            );

            // 3) Ensure biconnectivity: add a virtual edge between entrance and exit if missing
            var sIdx = indexOf[entrance];
            if (!workAdj[sIdx].Contains(exitIdx))
            {
                workAdj[sIdx].Add(exitIdx);
                workAdj[exitIdx].Add(sIdx);
            }

            // 4) Restrict to main biconnected component containing both endpoints
            RestrictToMainComponent(exitIdx, sIdx, adj, workVerts, workAdj);

            // 5) Iteratively remove source vertices using block-cutpoint decomposition
            var Q = new HashSet<int> { sIdx }; // current frontier of sources to remove
            int count = 0; // counts assigned st-numbers
            while (Q.Count > 0)
            {
                // Decompose working graph into biconnected components (BCCs) and find articulation points
                var bccList = new List<List<int>>();
                var artPts = new HashSet<int>();
                ComputeBiconnectedComponents(workVerts, workAdj, bccList, artPts);

                bool removed = false;
                // For each BCC, pick a removable source (not the exit or sole articulation)
                foreach (var comp in bccList)
                {
                    var cuts = comp.Where(v => artPts.Contains(v)).ToList();
                    int h = cuts.Count == 1 ? cuts[0]
                                              : (comp.Contains(exitIdx) ? exitIdx : -1);
                    if (h < 0) continue;
                    // Candidates: in this BCC, in Q, not h
                    var cand = comp.Where(v => v != h && Q.Contains(v)).ToList();
                    if (!cand.Any()) continue;

                    // Remove first candidate: assign next st-number
                    int vRem = cand[0];
                    stNumber[vRem] = ++count;
                    Q.Remove(vRem);
                    workVerts.Remove(vRem);

                    // Update frontier and adjacency: add neighbors (except exit) to Q
                    foreach (var u in workAdj[vRem].ToList())
                    {
                        if (u != exitIdx)
                            Q.Add(u);
                        workAdj[u].Remove(vRem);
                    }
                    workAdj.Remove(vRem);
                    removed = true;
                    break; // restart decomposition after change
                }
                if (!removed)
                    break; // no more sources can be removed
            }

            // 6) Finalize numbering for exit and orient edges based on st-numbers
            stNumber[exitIdx] = n;
            int oriented = 0;
            for (int u = 0; u < n; u++)
            {
                foreach (var v in adj[u])
                {
                    // Only consider each undirected pair once (u < v)
                    if (u < v && stNumber[u] > 0 && stNumber[v] > 0)
                    {
                        // Direct edge from lower to higher st-number
                        if (stNumber[u] < stNumber[v])
                            directedAdj[u].Add(v);
                        else
                            directedAdj[v].Add(u);
                        oriented++;
                    }
                }
            }
        }

        /// <summary>
        /// Breadth-first search from entrance to mark reachable vertices.
        /// </summary>
        private static bool[] ComputeReachable(
            List<Vector2I> cells,
            Dictionary<Vector2I, int> indexOf,
            List<HashSet<int>> adj,
            Vector2I entrance)
        {
            int n = cells.Count;
            var seen = new bool[n];
            if (!indexOf.TryGetValue(entrance, out var start))
                return seen;

            // Standard BFS
            var queue = new Queue<int>();
            seen[start] = true;
            queue.Enqueue(start);
            while (queue.Count > 0)
            {
                int u = queue.Dequeue();
                foreach (int v in adj[u])
                {
                    if (!seen[v])
                    {
                        seen[v] = true;
                        queue.Enqueue(v);
                    }
                }
            }
            return seen;
        }

        /// <summary>
        /// Restricts the working graph to the single BCC containing both entrance and exit.
        /// If no such BCC exists, leaves the full graph intact.
        /// </summary>
        private static void RestrictToMainComponent(
            int exitIdx,
            int sIdx,
            List<HashSet<int>> originalAdj,
            HashSet<int> workVerts,
            Dictionary<int, HashSet<int>> workAdj)
        {
            var allBCCs = new List<List<int>>();
            var artPts = new HashSet<int>();
            ComputeBiconnectedComponents(workVerts, workAdj, allBCCs, artPts);

            // Find the component that includes both endpoints
            var mainBCC = allBCCs.FirstOrDefault(c => c.Contains(sIdx) && c.Contains(exitIdx));
            if (mainBCC != null)
            {
                // Rebuild workVerts and workAdj to only include mainBCC nodes
                workVerts.Clear();
                foreach (var u in mainBCC) workVerts.Add(u);
                var newAdj = workVerts.ToDictionary(
                    u => u,
                    u => new HashSet<int>(originalAdj[u].Where(v => workVerts.Contains(v)))
                );
                workAdj.Clear();
                foreach (var kv in newAdj) workAdj[kv.Key] = kv.Value;
            }
        }

        /// <summary>
        /// Tarjan's algorithm to compute biconnected components and articulation points.
        /// </summary>
        private static void ComputeBiconnectedComponents(
            HashSet<int> verts,
            Dictionary<int, HashSet<int>> workAdj,
            List<List<int>> bccList,
            HashSet<int> artPoints)
        {
            int time = 0;
            var disc = verts.ToDictionary(u => u, u => -1);
            var low = verts.ToDictionary(u => u, u => 0);
            var parent = verts.ToDictionary(u => u, u => -1);
            var stack = new Stack<(int, int)>();

            // Recursive helper
            void BCCUtil(int u)
            {
                disc[u] = low[u] = ++time;
                int childCount = 0;

                foreach (var v in workAdj[u].ToList())
                {
                    if (disc[v] == -1)
                    {
                        childCount++;
                        parent[v] = u;
                        stack.Push((u, v));
                        BCCUtil(v);
                        low[u] = Math.Min(low[u], low[v]);

                        // Articulation condition
                        if ((parent[u] == -1 && childCount > 1) ||
                            (parent[u] != -1 && low[v] >= disc[u]))
                        {
                            artPoints.Add(u);
                            var comp = new List<int>();
                            (int, int) edge;
                            // Pop edges until this one
                            do
                            {
                                edge = stack.Pop();
                                if (!comp.Contains(edge.Item1)) comp.Add(edge.Item1);
                                if (!comp.Contains(edge.Item2)) comp.Add(edge.Item2);
                            } while (!(edge.Item1 == u && edge.Item2 == v));
                            bccList.Add(comp);
                        }
                    }
                    else if (v != parent[u] && disc[v] < disc[u])
                    {
                        // Back-edge found, update low-link
                        low[u] = Math.Min(low[u], disc[v]);
                        stack.Push((u, v));
                    }
                }
            }

            // Run DFS for each component
            foreach (var u in verts)
                if (disc[u] == -1)
                    BCCUtil(u);

            // Flush any remaining edges as a final BCC
            if (stack.Count > 0)
            {
                var comp = new List<int>();
                while (stack.Count > 0)
                {
                    var e = stack.Pop();
                    if (!comp.Contains(e.Item1)) comp.Add(e.Item1);
                    if (!comp.Contains(e.Item2)) comp.Add(e.Item2);
                }
                bccList.Add(comp);
            }
        }
    }
}
