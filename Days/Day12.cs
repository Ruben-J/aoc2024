using System.Numerics;
using System.Text;

namespace aoc2024.Days;

public static class Day12
{
    private static string[,] _matrix = new string[0, 0];
    private static Dictionary<string, HashSet<Vector2>> _areas = new Dictionary<string, HashSet<Vector2>>();
    private static Vector2 Up = new Vector2(0, 1);
    private static Vector2 Down = new Vector2(0, -1);
    private static Vector2 Right = new Vector2(1, 0);
    private static Vector2 Left = new Vector2(-1, 0);
    private static List<Vector2> _directions = new List<Vector2>
        {
            Up,
            Down,
            Right,
            Left
        };


    public static async Task Execute()
    {
        _matrix = await ReadMatrixFromFileAsync("Input/Day12.txt");
        for (var i = 0; i < _matrix.GetLength(0); i++)
        {
            for (var j = 0; j < _matrix.GetLength(1); j++)
            {
                // skip visited areas
                if (_areas.Any(x => x.Value.Any(y => y == new Vector2(i, j)))) continue;
                else
                {
                    var visited = new HashSet<Vector2>();
                    NavigateArea(new Vector2(i, j), ref visited);
                    _areas.Add($"{_matrix[i, j]}({i},{j})", visited);
                }
            }
        }
        var totalCostPart1 = _areas.Sum(x =>
        {
            static int selector(Vector2 y) => CalculateFence(y);
            var perimeter = x.Value.Sum(selector);
            return perimeter * x.Value.Count;
        });
        var totalCostPart2 = _areas.Sum(x =>
        {
            var sides = CalculateNumberOfSides(x.Value);
            return sides * x.Value.Count;;
        });
        Console.WriteLine($"Day 12: part1: {totalCostPart1} part2: {totalCostPart2}");
    }

    private static int CalculateNumberOfSides(HashSet<Vector2> value)
    {
        var sides = 0;
        foreach (var point in value)
        {
            if (!IsValidMove(point, point + Up) && !IsValidMove(point, point + Left)) sides++;
            if (!IsValidMove(point, point + Down) && !IsValidMove(point, point + Left)) sides++;
            if (!IsValidMove(point, point + Up) && !IsValidMove(point, point + Right)) sides++;
            if (!IsValidMove(point, point + Down) && !IsValidMove(point, point + Right)) sides++;

            if (IsValidMove(point, point + Up) && IsValidMove(point, point + Right) && !IsValidMove(point, point + Up + Right)) sides++;
            if (IsValidMove(point, point + Up) && IsValidMove(point, point + Left) && !IsValidMove(point, point + Up + Left)) sides++;
            if (IsValidMove(point, point + Down) && IsValidMove(point, point + Left) && !IsValidMove(point, point + Down + Left)) sides++;
            if (IsValidMove(point, point + Down) && IsValidMove(point, point + Right) && !IsValidMove(point, point + Down + Right)) sides++;
        }
        return sides;
    }

    private static int CalculateFence(Vector2 vector)
    {
        var fence = 4;
        foreach (var direction in _directions)
        {
            var nextPoint = vector + direction;

            if (IsValidMove(vector, nextPoint))
            {
                fence--;
            }
        }
        return fence;
    }

    private static void NavigateArea(Vector2 currentPoint, ref HashSet<Vector2> visited)
    {
        if (visited.Contains(currentPoint)) return;
        visited.Add(currentPoint);

        foreach (var direction in _directions)
        {
            var nextPoint = currentPoint + direction;
            if (IsValidMove(currentPoint, nextPoint))
            {
                NavigateArea(nextPoint, ref visited);
            }
        }
    }

    private static bool IsValidMove(Vector2 currentPoint, Vector2 nextPoint)
    {
        if (nextPoint.X < 0 || nextPoint.X >= _matrix.GetLength(0) ||
            nextPoint.Y < 0 || nextPoint.Y >= _matrix.GetLength(1))
        {
            return false;
        }
        return _matrix[(int)nextPoint.X, (int)nextPoint.Y] == _matrix[(int)currentPoint.X, (int)currentPoint.Y];
    }

    private static async Task<string[,]> ReadMatrixFromFileAsync(string filePath)
    {
        var lines = await File.ReadAllLinesAsync(filePath);
        string[,] matrix = new string[lines[0].Length, lines.Length];

        for (var i = 0; i < lines.Length; i++)
        {
            var characters = lines[i].ToCharArray();
            for (var j = 0; j < characters.Length; j++)
            {
                if (characters[j] == '.') continue;
                matrix[j, i] = characters[j].ToString();
            }
        }
        return matrix;
    }
}