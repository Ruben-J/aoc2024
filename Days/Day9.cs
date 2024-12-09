using System;
using System.Text;

namespace aoc2024.Days;

public static class Day9
{
    public static async Task Execute()
    {
        var text = (await File.ReadAllTextAsync("Input/Day9.txt")).ToCharArray();
        var diskInput = ParseDiskInput(text);
        var formattedDiskInput = FormatDiskInput(diskInput);
        var reversedDiskInput = diskInput.ToArray().Reverse().ToArray();
        var skipped = 0;
        Console.WriteLine(reversedDiskInput);

        var countDataRecords = 0;
        foreach (var dataInput in reversedDiskInput)
        {
            if (dataInput.Item1 != -1)
            {
                var matched = MatchDataInput(dataInput, formattedDiskInput, ref countDataRecords);
                if (!matched && skipped == 0)
                {
                    skipped = countDataRecords;
                }
            }
            countDataRecords += dataInput.Item2;
        }

        var total = CalculateTotal(formattedDiskInput);
        Console.WriteLine("Day 9: " + total);
    }

    private static List<(int, int)> ParseDiskInput(char[] text)
    {
        var added = 0;
        var isData = true;
        var diskInput = new List<(int, int)>();
        for (var i = 0; i < text.Length; i++)
        {
            var times = Convert.ToInt32(text[i].ToString());
            diskInput.Add((isData ? added : -1, times));
            if (isData) added++;
            isData = !isData;
        }
        return diskInput;
    }

    private static int[] FormatDiskInput(List<(int, int)> diskInput)
    {
        var formattedString = new List<int>();
        foreach (var (value, times) in diskInput)
        {
            formattedString.AddRange(Enumerable.Repeat(value, times));
        }
        return formattedString.ToArray();
    }

    private static bool MatchDataInput((int, int) dataInput, int[] formattedDiskInput, ref int countDataRecords)
    {
        int count = 0;
        int? start = null;
        for (var i = 0; i < formattedDiskInput.Length - countDataRecords; i++)
        {
            if (start != null && count == dataInput.Item2)
            {
                while (count > 0)
                {
                    formattedDiskInput[start.Value + (count - 1)] = dataInput.Item1;
                    formattedDiskInput[formattedDiskInput.Length - (countDataRecords + count)] = -1;
                    count--;
                }
                return true;
            }
            else if (formattedDiskInput[i] == -1)
            {
                if (start == null) start = i;
                count++;
            }
            else
            {
                count = 0;
                start = null;
            }
        }
        return false;
    }

    private static long CalculateTotal(int[] formattedDiskInput)
    {
        long total = 0;
        for (var i = 0; i < formattedDiskInput.Length; i++)
        {
            if (formattedDiskInput[i] != -1)
            {
                total += i * formattedDiskInput[i];
            }
        }
        return total;
    }
}
