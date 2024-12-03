using System;

namespace aoc2024.Days;

public static class Day1
{
    public static async Task Execute()
    {
        var lines = await File.ReadAllLinesAsync("Input/Day1.txt");
        var splitted = lines.Select(x => x.Split("   "));
        var firstList = splitted.Select(x => Convert.ToInt32(x[0])).OrderBy(x => x).ToArray();
        var secondList = splitted.Select(x => Convert.ToInt32(x[1])).OrderBy(x => x).ToArray();
        var similarity = firstList.Sum(x => x * secondList.Count(y => y == x));
        Console.WriteLine($"Day 1: {similarity}");
    }
}
