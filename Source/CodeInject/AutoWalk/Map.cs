using CodeInject.MemoryTools;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge;
using System.Drawing;
using Point = AForge.Point;

namespace CodeInject.AutoWalk
{
    public class Map
    {
        public Image OryginalMap;
        public Image RoadMap;

        public string OrginalMapFile { get; set; }
        public string RoadMapFile { get; set; }

        public double[,] GameCoords, MapCoords;


        private BellmanFordAlgorithm PathFinder;


        public Map(string oryginalMapFile, string roadMapFile, double[,] gameCoords, double[,] mapCoords)
        {
            this.OrginalMapFile = oryginalMapFile;
            this.RoadMapFile = roadMapFile;

            OryginalMap = new Bitmap(DataBase.DataPath + "\\Map\\" + oryginalMapFile);
            RoadMap = new Bitmap(DataBase.DataPath + "\\Map\\" + roadMapFile);

            GameCoords = gameCoords;
            MapCoords = mapCoords;


            PathFinder = new BellmanFordAlgorithm(CreateGraph(FindWhitePixels()));
        }


        public List<Point> FindShortestPathOnMap(Point start, Point end)
        {
            return PathFinder.BellmanFord(start, end);
        }

        List<Point> FindWhitePixels()
        {
            List<Point> whitePixels = new List<Point>();

            Bitmap bitmap = new Bitmap(RoadMap);

            int sizeC = 4;
            for (int x = 0; x < bitmap.Width - sizeC; x += sizeC)
            {
                for (int y = 0; y < bitmap.Height - sizeC; y += sizeC)
                {
                    Color pixelColor = bitmap.GetPixel(x + (sizeC / 2), y + (sizeC / 2));

                    if (pixelColor.R == 255 && pixelColor.G == 255 && pixelColor.B == 255)
                    {
                        whitePixels.Add(new Point(x + (sizeC / 2), y + (sizeC / 2)));
                    }
                }

            }

            return whitePixels;
        }

        public Vector2 CalculatePositionFromMap2World(float x, float y)
        {
            /* double[,] originalPoints = { { 5417, 5373 }, { 5651, 5368 }, { 5742, 5095 } };
             double[,] targetPoints = { { 162, 93 }, { 241, 97 }, { 272, 176 } };
            //ZANT
            double[,] originalPoints = { { 5240, 5192 }, { 5424, 5175 }, { 5276, 5432 } };
            double[,] targetPoints = { { 172, 227 }, { 249, 234 }, { 190, 131 } };
            */

            var characterCoordinatesInGame = GetCharacterCoordinatesInGame(x, y, GameCoords, MapCoords);

            return new Vector2((float)characterCoordinatesInGame.Item1, (float)characterCoordinatesInGame.Item2);
        }

        static Tuple<double, double> GetCharacterCoordinatesInGame(double clickedX, double clickedY, double[,] sourcePoints, double[,] targetPoints)
        {
            var clickedVector = Vector<double>.Build.DenseOfArray(new double[] { clickedX, clickedY, 1 });
            var inverseTransformationMatrix = FindAffineTransformationInverse(sourcePoints, targetPoints);
            var characterCoordinatesInGameVector = inverseTransformationMatrix.Multiply(clickedVector);

            var characterXInGame = characterCoordinatesInGameVector[0];
            var characterYInGame = characterCoordinatesInGameVector[1];

            return Tuple.Create(characterXInGame, characterYInGame);
        }


        static Matrix<double> FindAffineTransformationInverse(double[,] sourcePoints, double[,] targetPoints)
        {
            var transformationMatrix = FindAffineTransformation(sourcePoints, targetPoints);
            var inverseTransformationMatrix = transformationMatrix.Inverse();
            return inverseTransformationMatrix;
        }

        static Matrix<double> FindAffineTransformation(double[,] sourcePoints, double[,] targetPoints)
        {
            var sourceMatrix = DenseMatrix.OfArray(new double[,]
            {
                    { sourcePoints[0, 0], sourcePoints[0, 1], 1 },
                    { sourcePoints[1, 0], sourcePoints[1, 1], 1 },
                    { sourcePoints[2, 0], sourcePoints[2, 1], 1 }
            });

            var targetMatrix = DenseMatrix.OfArray(new double[,]
            {
                    { targetPoints[0, 0], targetPoints[0, 1], 1 },
                    { targetPoints[1, 0], targetPoints[1, 1], 1 },
                    { targetPoints[2, 0], targetPoints[2, 1], 1 }
            });

            var coefficients = sourceMatrix.Solve(targetMatrix);

            return DenseMatrix.OfArray(new double[,]
            {
                    { coefficients[0, 0], coefficients[1, 0], coefficients[2, 0] },
                    { coefficients[0, 1], coefficients[1, 1], coefficients[2, 1] },
                    { 0, 0, 1 }
            });
        }


        static Dictionary<Point, List<Point>> CreateGraph(List<Point> vertices)
        {
            var graph = new Dictionary<Point, List<Point>>();

            foreach (var vertex in vertices)
            {
                var neighbors = vertices.FindAll(p => IsNeighbor(p, vertex));
                graph[vertex] = neighbors;
            }

            return graph;
        }

        static bool IsNeighbor(Point p1, Point p2)
        {
            return Math.Abs(p1.X - p2.X) <= 4 && Math.Abs(p1.Y - p2.Y) <= 4;
        }

        public unsafe Point PlayerPositionOnMap()
        {
            /* 
             double[,] originalPoints = { { 5417, 5373 }, { 5651, 5368 }, { 5742, 5095 } };
             double[,] targetPoints = { { 162, 93 }, { 241, 97 }, { 272, 176 } };
            
            //ZANT
            double[,] originalPoints = { { 5240, 5192 }, { 5424, 5175 }, { 5276, 5432 } };
            double[,] targetPoints = { { 172, 227 }, { 249, 234 }, { 190, 131 } };
            */

            var transformationMatrix = FindAffineTransformation(GameCoords, MapCoords);



            var characterCoordinates = new double[] { *GameFunctionsAndObjects.DataFetch.GetPlayer().X / 100, *GameFunctionsAndObjects.DataFetch.GetPlayer().Y / 100, 1 };
            var newCoordinates = transformationMatrix.Multiply(Vector<double>.Build.DenseOfArray(characterCoordinates));
            return new Point((float)newCoordinates[0], (float)newCoordinates[1]);
        }

    }
}
