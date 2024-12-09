using System;
using System.Reflection.PortableExecutable;

namespace aoc2024.Days;

public static class Day6
{
    public static async Task Execute()
    {
        char Guard = '^';
        var lines = await File.ReadAllLinesAsync("Input/Day6.txt");
        char[,] matrix = new char[lines[0].Length, lines.Length];
        int GuardX = 0;
        int GuardY = 0;


        for (var i = 0; i < lines.Length; i++)
        {
            var characters = lines[i].ToCharArray();
            for (var j = 0; j < characters.Length; j++)
            {
                matrix[j, i] = characters[j];
                if (matrix[j, i] == Guard)
                {
                    GuardX = j;
                    GuardY = i;
                }
            }
        }
        
        var obstacle = 0;
        for(var i = 0; i < matrix.GetLength(0); i++){
            for(var j = 0; j < matrix.GetLength(1); j++){
                Console.WriteLine($"i: {i} j: {j}, obstacles: {obstacle}, percentage: {((double)i/matrix.GetLength(0))*100}");
                if(matrix[i,j] == '.'){
                    var cloneMatrix = matrix.Clone() as char[,];
                    cloneMatrix[i,j] = '#';
                   
                    if(SimulateMatrix(Guard, lines, cloneMatrix, GuardX, GuardY)){
                        obstacle++;
                    }
                }
            }
        }
        Console.WriteLine($"Day 6: {obstacle}");
    }

    private static bool SimulateMatrix(char Guard, string[] lines, char[,] matrix, int GuardX, int GuardY)
    {
        bool guardIsOut = false;
        List<(int x, int y, char guard)> visited = new List<(int x, int y, char guard)>();
        
        while (!guardIsOut)
        {
            var (newX, newY) = DetermineMove(Guard, GuardX, GuardY);
            if (newX == lines[0].Length || newX < 0 || newY == lines.Length || newY < 0)
            {
                matrix[GuardX, GuardY] = 'X';
                guardIsOut = true;
            }
            else if(visited.Contains((newX,newY,Guard))){
                return true;
            }
            else if (matrix[newX, newY] == '#')
            {
                Guard = TurnRight(Guard);
            }
            else
            {
                matrix[GuardX, GuardY] = 'X';
                GuardX = newX;
                GuardY = newY;
                matrix[GuardX, GuardY] = Guard;
                visited.Add((GuardX,GuardY, Guard));
            }
        }
        return false;
    }

    private static char TurnRight(char Guard)
    {
        switch(Guard){
            case '^':
                return '>';
            case 'v':
                return  '<';
            case '>':
                return 'v';
            case '<':
                return '^';
            default:
                return Guard;
        }
    }

    private static (int x,int y) DetermineMove(char Guard, int guardX, int guardY){

        switch(Guard){
            case '^':
                return (guardX,guardY - 1);
            case 'v':
                return (guardX,guardY +1);
            case '>':
                return (guardX+1, guardY);
            case '<':
                return (guardX -1,guardY);
            default:
                return (guardX,guardY);
        }
    }
}
