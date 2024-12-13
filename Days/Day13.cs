using System;
using Microsoft.VisualBasic;

namespace aoc2024.Days;

public class Day13
{

    public static async Task Execute()
    {
        var lines = await File.ReadAllLinesAsync("Input/Day13.txt");
        var equotations = new List<Equotation>();
        for(int i = 0; i < lines.Length; i+=4)
        {
            var equotation = new Equotation()
            {
                Ax = int.Parse(lines[i].Split('+')[1].Split(',')[0]),
                Ay =  int.Parse(lines[i].Split('+')[2]),
                Bx = int.Parse(lines[i+1].Split('+')[1].Split(',')[0]),
                By = int.Parse(lines[i+1].Split('+')[2]),
                PrizeX = 10000000000000 + Int64.Parse(lines[i+2].Split('=')[1].Split(',')[0]),
                PrizeY = 10000000000000 + Int64.Parse(lines[i+2].Split('=')[2]),
            };
            equotations.Add(equotation);    
        }
        long totalCoins = 0;
        foreach(var equotation in equotations)
        {
            var (a, b)  = (equotation.PrizeY * equotation.Bx - equotation.PrizeX * equotation.By, equotation.Ay * equotation.Bx - equotation.Ax * equotation.By);
            var c = equotation.PrizeX - (a/b) * equotation.Ax;
            if (a % b == 0 && c % equotation.Bx == 0)
                totalCoins += 3 * (a/b) + (c / equotation.Bx);
        }      

        Console.WriteLine($"Day 13: {totalCoins}");
    }
}

public class Equotation{
    public int Ax { get; set; }
    public int Ay { get; set; }
    public int Bx { get; set; }
    public int By { get; set; }

    public long PrizeX {get; set;}
    public long PrizeY {get;set;}
}