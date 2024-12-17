using System;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;

namespace aoc2024.Days;

public static class Day16
{

    private static Vector2 Start;
    private static Vector2 End;
    private static char[,] Maze;

    private static Vector2 North = new Vector2(0, -1);
    private static Vector2 South = new Vector2(0, 1);
    private static Vector2 East = new Vector2(1, 0);
    private static Vector2 West = new Vector2(-1, 0);
    public static async Task Execute()
    {
        var lines = await File.ReadAllLinesAsync("Input/Day16.txt");
        Maze = new char[lines[0].Length, lines.Length];
        for (int i = 0; i < lines.Length; i++)
        {
            for (int j = 0; j < lines[0].Length; j++)
            {
                Maze[j, i] = lines[i][j];
                if (Maze[j, i] == 'S')
                    Start = new Vector2(j, i);
                if (Maze[j, i] == 'E')
                    End = new Vector2(j, i);
            }
        }

        var routes = FindBestRoute();

        var bestRoute = routes.OrderBy(x => x.Item3).FirstOrDefault(x => x.Item1 == End);
        var allTiles = new List<(Vector2,Vector2)>(bestRoute.Item4);

        for( int i = 0; i < bestRoute.Item4.Count - 1; i++){
            var route = routes.FirstOrDefault(x => x.Item1 == bestRoute.Item4[i].Item1 && x.Item2 == bestRoute.Item4[i+1].Item2);
            var alternative = routes.FirstOrDefault(x => x.Item3 == route.Item3 - 1000);
            if(route.Item3 != 0)
                allTiles.AddRange(route.Item4);
            
        }
        var tiles = allTiles.DistinctBy(x => x.Item1).ToList();
        PrintAllTilesOnMaze(tiles);

        Console.WriteLine($"Day 16: Cost: {bestRoute.Item3}  Tiles: {tiles.Count()}");
    }

    private static void PrintAllTilesOnMaze(List<(Vector2,Vector2)> tiles){
        for(int i = 0; i < Maze.GetLength(1); i++){
            for(int j = 0; j < Maze.GetLength(0); j++){
                if(tiles.Any(x => x.Item1 == new Vector2(j,i)))
                    Console.Write('O');
                else
                    Console.Write(Maze[j,i]);
            }
            Console.WriteLine();
        }
    }

    private static List<(Vector2, Vector2, int, List<(Vector2,Vector2)>)> FindBestRoute()
    {
        List<(Vector2, Vector2, int, List<(Vector2,Vector2)>)> openPoints = new List<(Vector2, Vector2, int, List<(Vector2,Vector2)>)>() {
            
            //(Start, North, 1000, new List<(Vector2,Vector2)>(){(Start, North)}),
            (Start, East, 0, new List<(Vector2,Vector2)>(){(Start, East)}),
            
        };
        List<(Vector2, Vector2, int, List<(Vector2, Vector2)>)> routes = new List<(Vector2, Vector2, int, List<(Vector2,Vector2)>)>();
        while (openPoints.Count > 0)
        {
            
            var cost = 1;
            var openPoint = openPoints[0];
            openPoints.RemoveAt(0);
            var newPoint = openPoint.Item1 + openPoint.Item2;
            var visited = openPoint.Item4;
            visited.Add((newPoint,openPoint.Item2));
            var otherWay = openPoint.Item2 * -1;
            var possibleWays = new Vector2[] { East, West, North, South }.Where(x => x != otherWay && Maze[(int)(newPoint + x).X, (int)(newPoint + x).Y] == '.').ToList();
            var previousWay = openPoint.Item2;
            while (possibleWays.Count == 1)
            {
                cost += previousWay == possibleWays[0] ? 1 : 1001;
                newPoint += possibleWays[0];
                visited.Add((newPoint,possibleWays[0]));
                if (newPoint == End)
                {
                    var route = routes.FirstOrDefault(x => x.Item1 == newPoint && x.Item2 == possibleWays[0]);
                    if(route.Item3 == 0){    
                         routes.Add((newPoint, possibleWays[0], openPoint.Item3 + cost, visited));
                    }
                    else if(route.Item3 == openPoint.Item3 + cost){
                        route.Item4.AddRange(visited);
                    }
                    else{
                        routes.Remove(route);
                        routes.Add((newPoint, possibleWays[0], openPoint.Item3 + cost, visited));
                    }
                    
                    possibleWays = new List<Vector2>();
                    break;
                }

                otherWay = possibleWays[0] * -1;
                previousWay = possibleWays[0];
                possibleWays = new Vector2[] { East, West, North, South }.Where(x => x != otherWay && (Maze[(int)(newPoint + x).X, (int)(newPoint + x).Y] == '.' || newPoint + x == End)).ToList();
            }

            if (possibleWays.Count > 1)
            {
                foreach (var possibleWay in possibleWays)
                {
                    var totalCost = openPoint.Item3 + cost + ((possibleWay != previousWay) ? 1000 : 0);
                    if (routes.Any(x => x.Item1 == newPoint && x.Item2 == possibleWay && x.Item3 < totalCost))
                        continue;
                    else if (routes.Any(x => x.Item1 == newPoint && x.Item2 == possibleWay && x.Item3 == totalCost))
                    {
                        var route = routes.FirstOrDefault(x => x.Item1 == newPoint && x.Item2 == possibleWay);
                        route.Item4.AddRange(visited);
                        continue;
                    }
                    else if (routes.Any(x => x.Item1 == newPoint && x.Item2 != possibleWay && x.Item3 == totalCost + 1000))
                    {
                        var route = routes.FirstOrDefault(x => x.Item1 == newPoint && x.Item2 != possibleWay && x.Item3 == totalCost + 1000);
                        route.Item4.AddRange(visited);
                        
                    }
                    
                        var currentRoute = routes.FirstOrDefault(x => x.Item1 == newPoint && x.Item2 == possibleWay);
                        if(currentRoute.Item3 != 0) routes.Remove(currentRoute);
                        routes.Add((newPoint, possibleWay, totalCost, [..visited]));
                        openPoints.Add((newPoint, possibleWay, totalCost,[..visited]));
                    
                }
            }
        }

        return routes;
    }

}
