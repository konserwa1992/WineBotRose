using AForge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CodeInject.AutoWalk
{
    internal class BellmanFordAlgorithm
    {
        private Dictionary<Point, List<Point>> adjacencyList;
        private Dictionary<Point, Point?> previousVertices;

        public BellmanFordAlgorithm(Dictionary<Point, List<Point>> graph)
        {
            adjacencyList = graph;
            previousVertices = new Dictionary<Point, Point?>();
        }

        public List<Point> BellmanFord(Point startVertex, Point endVertex)
        {
            int[] distance = new int[adjacencyList.Count];
            List<Point> vertices = new List<Point>(adjacencyList.Keys);

            for (int i = 0; i < distance.Length; ++i)
            {
                distance[i] = int.MaxValue;
                previousVertices[vertices[i]] = null;
            }


            var closestStartPoint2Character = adjacencyList.OrderBy(x => Vector2.Distance(new Vector2(x.Key.X, x.Key.Y), new Vector2(startVertex.X, startVertex.Y))).FirstOrDefault();

            var closestEndPoint2Character = adjacencyList.OrderBy(x => Vector2.Distance(new Vector2(x.Key.X, x.Key.Y), new Vector2(endVertex.X, endVertex.Y))).FirstOrDefault();


            int index = vertices.IndexOf(closestStartPoint2Character.Key);
            distance[index] = 0;

            for (int i = 0; i < adjacencyList.Count - 1; ++i)
            {
                foreach (var vertex in vertices)
                {
                    foreach (var destination in adjacencyList[vertex])
                    {
                        int vertexIndex = vertices.IndexOf(vertex);
                        int destinationIndex = vertices.IndexOf(destination);

                        if (distance[vertexIndex] != int.MaxValue &&
                            distance[vertexIndex] + 1 < distance[destinationIndex])
                        {
                            distance[destinationIndex] = distance[vertexIndex] + 1;
                            previousVertices[destination] = vertex;
                        }
                    }
                }
            }

            // Sprawdzanie cykli ujemnych
            foreach (var vertex in vertices)
            {
                foreach (var destination in adjacencyList[vertex])
                {
                    int vertexIndex = vertices.IndexOf(vertex);
                    int destinationIndex = vertices.IndexOf(destination);

                    if (distance[vertexIndex] != int.MaxValue &&
                        distance[vertexIndex] + 1 < distance[destinationIndex])
                    {
                        return new List<Point>();
                    }
                }
            }

            return PrintShortestPath(closestStartPoint2Character.Key, closestEndPoint2Character.Key);
        }


        private List<Point> PrintShortestPath(Point startVertex, Point endVertex)
        {
            List<Point> path = new List<Point>();
            Point currentVertex = endVertex;

            while (currentVertex != null)
            {
                path.Insert(0, currentVertex);
                if (previousVertices[currentVertex] != null)
                {
                    currentVertex = (Point)previousVertices[currentVertex];
                }
                else
                {
                    break;
                }
            }

            return path;
        }

        public List<Point> FindShortestPath(Point start, Point end)
        {
            return BellmanFord(start, end);
        }
    }
}
