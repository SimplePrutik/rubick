using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Map
{
    public class MazeGenerator : MonoBehaviour
    {
        public struct Cell
        {
            public bool Top;
            public bool Bottom;
            public bool Left;
            public bool Right;
            public bool Front;
            public bool Back;
        }
    
        private struct CellPoint
        {
            public int X;
            public int Y;
            public int Z;
        }

        [SerializeField] private CellObject cellPrefab;
        [SerializeField] private int mazeSize;
        [SerializeField] private Vector3 cellSize;

        private Cell[,,] maze;
        private List<(CellPoint, CellPoint)> edges = new List<(CellPoint, CellPoint)>();
        private List<List<CellPoint>> forests = new List<List<CellPoint>>();

        [Inject]
        public void Construct(PlayerController playerController)
        {
            Generate();
            
            playerController.transform.localPosition = Vector3.zero;
        }

        private void Generate()
        {
            maze = new Cell[mazeSize, mazeSize, mazeSize];
        
            for (var i = 0; i < mazeSize; i++)
            {
                for (var j = 0; j < mazeSize; j++)
                {
                    for (var k = 0; k < mazeSize; k++)
                    {
                        if (i < mazeSize - 1)
                            edges.Add((new CellPoint{X = i, Y = j, Z = k}, new CellPoint{X = i + 1, Y = j, Z = k}));
                        if (j < mazeSize - 1)
                            edges.Add((new CellPoint{X = i, Y = j, Z = k}, new CellPoint{X = i, Y = j + 1, Z = k}));
                        if (k < mazeSize - 1)
                            edges.Add((new CellPoint{X = i, Y = j, Z = k}, new CellPoint{X = i, Y = j, Z = k + 1}));
                    }
                }
            }

            edges = edges.OrderBy(_ => Guid.NewGuid()).ToList();

            foreach (var edge in edges)
            {
                // Debug.Log("////// FORESTS ///////");
                // foreach (var forest in forests)
                // {
                //     Debug.Log(string.Join(", ", forest.Select(cell => $"({cell.X}, {cell.Y}, {cell.Z})").ToList()));
                // }
                // Debug.Log("////// FORESTS ///////");
            
                var forest1 = forests.Find(forest => forest.Contains(edge.Item1));
                var forest2 = forests.Find(forest => forest.Contains(edge.Item2));
                if (forest1 != null)
                {
                    if (forest2 == forest1)
                    {
                        // Debug.Log($"Edge ({edge.Item1.X}, {edge.Item1.Y}, {edge.Item1.Z}) - ({edge.Item2.X}, {edge.Item2.Y}, {edge.Item2.Z}) : Denied");
                        continue;
                    }
                    if (forest2 == null)
                    {
                        forest1.Add(edge.Item2);
                    }
                    else
                    {
                        forest1.AddRange(forest2);
                        forests.Remove(forest2);
                    }
                }
                else
                {
                    if (forest2 != null)
                    {
                        forest2.Add(edge.Item1);
                    }
                    else
                    {
                        forests.Add(new List<CellPoint>{edge.Item1, edge.Item2});
                    }
                }

                SetPassage(edge.Item1, edge.Item2);
                // Debug.Log($"Edge ({edge.Item1.X}, {edge.Item1.Y}, {edge.Item1.Z}) - ({edge.Item2.X}, {edge.Item2.Y}, {edge.Item2.Z}) : Accepted");
            }
        
        
        
            for (var i = 0; i < mazeSize; i++)
            for (var j = 0; j < mazeSize; j++)
            for (var k = 0; k < mazeSize; k++)
            {
                var cell = Instantiate(cellPrefab, new Vector3(i * 10f * cellSize.x, j * 10f * cellSize.y, k * 10f * cellSize.z), Quaternion.identity);
                cell.SetWalls(maze[i, j, k]);
                cell.SetSize(cellSize);
                cell.transform.SetParent(transform);
            }
        }

        private void SetPassage(CellPoint cp1, CellPoint cp2)
        {
            var side = new CellPoint {X = cp1.X - cp2.X, Y = cp1.Y - cp2.Y, Z = cp1.Z - cp2.Z};
            if (side.X == 1)
            {
                maze[cp1.X, cp1.Y, cp1.Z].Back = true;
                maze[cp2.X, cp2.Y, cp2.Z].Front = true;
            }
            if (side.X == -1)
            {
                maze[cp1.X, cp1.Y, cp1.Z].Front = true;
                maze[cp2.X, cp2.Y, cp2.Z].Back = true;
            }
            if (side.Y == 1)
            {
                maze[cp1.X, cp1.Y, cp1.Z].Bottom = true;
                maze[cp2.X, cp2.Y, cp2.Z].Top = true;
            }
            if (side.Y == -1)
            {
                maze[cp1.X, cp1.Y, cp1.Z].Top = true;
                maze[cp2.X, cp2.Y, cp2.Z].Bottom = true;
            }
            if (side.Z == 1)
            {
                maze[cp1.X, cp1.Y, cp1.Z].Right = true;
                maze[cp2.X, cp2.Y, cp2.Z].Left = true;
            }
            if (side.Z == -1)
            {
                maze[cp1.X, cp1.Y, cp1.Z].Left = true;
                maze[cp2.X, cp2.Y, cp2.Z].Right = true;
            }
        }
    }
}
