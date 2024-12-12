using System;
using System.Numerics;

namespace aoc2024.Days;

public static class Day10
{
     private static int?[,] _matrix = new int?[0,0];

    public static async Task Execute()
    {
        _matrix = await ReadMatrixFromFileAsync("Input/Day10.txt");
        var startPoints = GetStartPoints();
        var totalRoutes = 0;
        var totalPeaks = 0;
        
        foreach(var startPoint in startPoints){
           
            var peaksVisited = new HashSet<Vector2>();
            var subtotal = CountRoutesToNine(startPoint, ref peaksVisited);
            totalRoutes += subtotal;
            totalPeaks += peaksVisited.Count;
        }
        Console.WriteLine($"Day 10: Routes: {totalRoutes} Peaks: {totalPeaks}");
    }

    

    private static int CountRoutesToNine(Vector2 startPoint, ref HashSet<Vector2> visited)
    {
        return NavigateTrail(startPoint,ref visited);
    }

    private static int NavigateTrail(Vector2 currentPoint, ref HashSet<Vector2> visited)
    {
        if (_matrix[(int)currentPoint.X, (int)currentPoint.Y] == 9)
        {
            visited.Add(currentPoint);
            return 1;
        }

        int routes = 0;
        var directions = new List<Vector2>
        {
            new Vector2(0, 1), // up
            new Vector2(0, -1), // down
            new Vector2(1, 0), // right
            new Vector2(-1, 0) // left
        };

        foreach (var direction in directions)
        {
            var nextPoint = currentPoint + direction;
            if (IsValidMove(currentPoint, nextPoint, visited))
            {
                routes += NavigateTrail(nextPoint, ref visited);
            }
        }

        return routes;
    }

    private static bool IsValidMove(Vector2 currentPoint, Vector2 nextPoint, HashSet<Vector2> visited)
    {
        if (nextPoint.X < 0 || nextPoint.X >= _matrix.GetLength(0) ||
            nextPoint.Y < 0 || nextPoint.Y >= _matrix.GetLength(1))
        {
            return false;
        }


        return _matrix[(int)nextPoint.X, (int)nextPoint.Y] == _matrix[(int)currentPoint.X, (int)currentPoint.Y] + 1;
    }

    private static List<Vector2> GetStartPoints(){
        var result = new List<Vector2>();
        for (var i = 0; i < _matrix.GetLength(0); i++)
        {
            for (var j = 0; j < _matrix.GetLength(1); j++)
            {
                if(_matrix[i,j] == 0){
                   result.Add(new Vector2(i,j));
                }
            }
        }
        return result;
    }

     private static async Task<int?[,]> ReadMatrixFromFileAsync(string filePath)
    {
        var lines = await File.ReadAllLinesAsync(filePath);
        int?[,] matrix = new int?[lines[0].Length, lines.Length];

        for (var i = 0; i < lines.Length; i++)
        {
            var characters = lines[i].ToCharArray();
            for (var j = 0; j < characters.Length; j++)
            {
                if(characters[j]=='.')continue;
                matrix[j, i] = Convert.ToInt32(characters[j].ToString());
            }
        }
        return matrix;
    }
}
