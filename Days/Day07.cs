using System;

namespace aoc2024.Days;

public static class Day07
{
    static Dictionary<Int64, List<Int64>> equotations = new();
    public static async Task Execute()
    {
        var lines = await File.ReadAllLinesAsync("Input/Day07.txt");
        equotations = lines.ToDictionary(x => Convert.ToInt64(x.Split(':')[0]), x => x.Split(':')[1].Trim().Split(' ').Select(x => Convert.ToInt64(x)).ToList());
        var total = equotations.Where(x => ValidateEquotation(x.Key, x.Value)).Sum(x => x.Key);
        Console.WriteLine($"Day 7: {total}");
    }

    private static bool ValidateEquotation(Int64 total, List<Int64> parts)
    {

        var otherParts = parts.Skip(2).ToList();
        var resultPlus = parts[0] + parts[1];
        var resultMultiply = parts[0] * parts[1];
        var resultConcat = Convert.ToInt64($"{parts[0]}{parts[1]}");

        if (otherParts.Count == 0 && (resultPlus == total || resultMultiply == total || resultConcat == total))
        {
            return true;
        }
        if (resultPlus > total && resultMultiply > total && resultConcat > total)
        {
            return false;
        }
        if (otherParts.Count == 0)
        {
            return false;
        }

        if (ValidateOperationResult(resultPlus, total, otherParts) ||
        ValidateOperationResult(resultMultiply, total, otherParts) ||
        ValidateOperationResult(resultConcat, total, otherParts))
        {
            return true;
        }

        return false;
    }

    private static bool ValidateOperationResult(Int64 partResult, Int64 total, List<Int64> parts)
    {
        if (partResult <= total)
        {
            parts.Insert(0, partResult);
            if (ValidateEquotation(total, parts))
            {
                return true;
            }
            parts.RemoveAt(0);
        }
        return false;
    }
}
