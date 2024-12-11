using System.Text;

namespace aoc2024.Days;

public static class Day11
{
    public static async Task Execute()
    {
        var content = await File.ReadAllTextAsync("Input/Day11.txt");
        var digits = content.Split(" ").GroupBy(x => x).ToDictionary(x => x.Key, x => Convert.ToInt64(x.Count()));
        
        for (int i = 0; i < 75; i++)
        {   
            var dictionary = new Dictionary<string, long>();
            var stringBuilder = new StringBuilder();
            foreach (var digit in digits)
            {
                if (digit.Key == "0")
                {
                    AddToDictionary(dictionary, "1", digit.Value);
                }
                else if (digit.Key.Length % 2 == 0)
                {
                    var digitOne = RemoveMultipleLeadingZeros(digit.Key.Substring(0, digit.Key.Length / 2));
                    AddToDictionary(dictionary, digitOne, digit.Value);

                    var digitTwo = RemoveMultipleLeadingZeros(digit.Key.Substring(digit.Key.Length / 2)).ToString();
                    AddToDictionary(dictionary, digitTwo, digit.Value);
                }
                else
                {
                    AddToDictionary(dictionary, (Convert.ToInt64(digit.Key) * 2024).ToString(), digit.Value);
                }
            }
            digits = dictionary;
        }

        Console.WriteLine($"Day 11: {digits.Sum(x => x.Value)}");
    }

    private static void AddToDictionary(Dictionary<string, long> dictionary, string key, long value)
    {
        if (dictionary.ContainsKey(key))
        {
            dictionary[key] += value;
        }
        else
        {
            dictionary.Add(key, value);
        }
    }

    private static string RemoveMultipleLeadingZeros(string number)
    {
        if (string.IsNullOrEmpty(number)) return "0";
        int i = 0;
        while (i < number.Length - 1 && number[i] == '0')
        {
            i++;
        }
        return number.AsSpan(i).ToString();
    }
}