using System;
using System.Text.RegularExpressions;

namespace aoc2024.Days;

public static class Day4
{
    public static async Task Execute()
    {
        var lines = (await File.ReadAllLinesAsync("Input/Day4.txt")).ToArray();
        var horizontalCharacters = lines.Select(x => x.ToCharArray()).ToArray();
        var countXmas = 0;
        for (var x = 1; x < horizontalCharacters[0].Length - 1; x++)
        {
            for (var y = 1; y < horizontalCharacters.Length - 1; y++)
            {
                if (IsXmas(horizontalCharacters, x, y)) countXmas++;
            }
        }
        Console.WriteLine($"Day 4: {countXmas}");
    }

    public static bool IsXmas(char[][] matrix, int x, int y)
    {

        return matrix[y][x] == 'A' &&
         (
            (matrix[y - 1][x - 1] == 'M' && matrix[y + 1][x + 1] == 'S') ||
            (matrix[y - 1][x - 1] == 'S' && matrix[y + 1][x + 1] == 'M')
         ) &&
         (
            (matrix[y - 1][x + 1] == 'M' && matrix[y + 1][x - 1] == 'S') ||
            (matrix[y - 1][x + 1] == 'S' && matrix[y + 1][x - 1] == 'M')
         );
    }
}
