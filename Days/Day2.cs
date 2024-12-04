namespace aoc2024.Days;

public static class Day2
{
    public static async Task Execute()
    {
        var lines = await File.ReadAllLinesAsync("Input/Day2.txt");
        var safe = 0;
        foreach (var line in lines)
        {
            var numbers = line.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(x => Convert.ToInt32(x)).ToArray();
            if(IsValidLine(numbers, true))
                safe++;
        }
        Console.WriteLine($"Day 2: {safe}");
    }

    private static bool IsValidLine(int[] numbers, bool problemDamper = false)
    {
        bool valid = true;
        bool increasing = numbers[0] < numbers[1];
        for (int i = 0; i < numbers.Length - 1; i++)
        {
            var isValid = Validate(numbers[i], numbers[i + 1], increasing);
            if(!isValid){
                if(problemDamper && 
                    (IsValidLine(numbers.Where((x,index) => i != index).ToArray()) ||
                     IsValidLine(numbers.Where((x,index) => i + 1 != index).ToArray()))){
                        break;
                    };
                valid = false;
            }
        }
        return valid;
    }

    private static bool Validate(int a, int b, bool increasing)
    {
        if (a == b) return false;
        if (increasing && a > b) return false;
        if (!increasing && a < b) return false;
        if (Math.Abs(a - b) > 3) return false;
        return true;
    }
}
