using System.Text.RegularExpressions;

namespace aoc2024.Days;

public static class Day03
{
    public static async Task Execute()
    {
        var mulText = await File.ReadAllTextAsync("Input/Day03.txt");
        var regexMiddleParts = new Regex("(do\\(\\))(.|\\s)*?(don't\\(\\))");
        var result = regexMiddleParts.Matches($"do(){mulText}don't()").Sum(x => Multiply(x.Value));

        Console.WriteLine($"Day 3: {result}");
    }

    private static int Multiply(string mulText){
        var regex = new Regex("mul\\([0-9]*,[0-9]*\\)");
        var matches = regex.Matches(mulText);
        var result = matches.Select(match => match.Value.Substring(4, match.Value.Length - 5)
                            .Split(",").Select(x => Convert.ToInt32(x)).ToArray())
                            .Sum(x => x[0] * x[1]);
        return result;
    }
}
