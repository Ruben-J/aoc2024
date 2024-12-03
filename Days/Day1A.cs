using System;
using System.IO;

namespace aoc2024.Days;
public static class Day1A{
    public static void Execute(){
        var lines = File.ReadAllLines("Input/Day1.txt");
        var splitted = lines.Select(x => x.Split("   "));
        var firstList = splitted.Select(x => Convert.ToInt32(x[0])).OrderBy(x => x).ToArray();
        var secondList = splitted.Select(x => Convert.ToInt32(x[1])).OrderBy(x => x).ToArray();
        var distance = 0;
        for(var i = 0; i < secondList.Length; i++){
            distance += Math.Abs(firstList[i] - secondList[i]);
        }
        Console.WriteLine($"Day 1: {distance}");
    }
}



