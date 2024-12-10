using System;

namespace aoc2024.Days;

public static class Day05
{

    private static Dictionary<string,List<string>> _conditions = new();
    public static async Task Execute()
    {
        var lines = await File.ReadAllLinesAsync("Input/Day05.txt");
        bool checkLines = false;
        int total = 0;
        foreach(var line in lines){
            if(string.IsNullOrEmpty(line))
            {
                checkLines = true;
                continue;
            }
            if(checkLines)
            {
                var pages = line.Split(",").ToList();
                if(pages.Any(page => !ValidatePage(page, pages))){

                    var orderPages = new string[pages.Count];
                    foreach(var page in pages){
                        for ( int j = 0; j < pages.Count; j++)
                        {
                             var tempPages = orderPages.AsEnumerable().ToList();
                             tempPages.Insert(j, page);
                             if(ValidatePage(page, tempPages)){
                                orderPages = tempPages.ToArray();
                                break;
                             }
                        }
                    }
                    total+= Convert.ToInt32(orderPages[pages.Count/2]);
                }
            }
            else
            {
                ParseCondition(line);
            }
        }
        Console.WriteLine($"Day 5: {total}");
    }

    private static bool ValidatePage(string page, List<string> pages)
    {
        var index = pages.IndexOf(page);
        var pagesBefore = _conditions.Where(x => x.Value.Any(y => y == page)).Select(x => x.Key).ToArray();
        return pages.Skip(index+1).Where(x => x != null).All(x => !(_conditions.ContainsKey(x) && _conditions[x].Contains(page))) &&
            (index == 0 || pages.Take(index).All(x => !(_conditions.ContainsKey(page) && _conditions[page].Contains(x))));
    }

    private static void ParseCondition(string line)
    {
        var conditionStatement = line.Split("|");
        if (_conditions.ContainsKey(conditionStatement[0]))
            _conditions[conditionStatement[0]].Add(conditionStatement[1]);
        else
            _conditions.Add(conditionStatement[0], new List<string> { conditionStatement[1] });
    }
}
