using System;
using System.Numerics;

namespace aoc2024.Days;

public class Day14
{

    private static int Width = 101;
    private static int Height = 103;
    private static int Minutes = 7680;

    public static async Task Execute(){
        var lines = await File.ReadAllLinesAsync("Input/Day14.txt");
        var robots = new List<Robot>();
        foreach(var line in lines){
            var position = line.Split(" ")[0].Split("=")[1].Split(",");
            var velocity = line.Split(" ")[1].Split("=")[1].Split(",");
            robots.Add(new Robot(){
                Position = new Vector2(int.Parse(position[0]), int.Parse(position[1])),
                Velocity = new Vector2(int.Parse(velocity[0]), int.Parse(velocity[1]))
            });
        }
        List<(int,int)> results = new List<(int,int)>();
        for(int i = 1; i < Minutes; i++){
            foreach(var robot in robots){
                MoveRobot(robot);
            }
            //check density from center -/+ 20
            results.Add((i, robots.Count(x =>   x.Position.X <= Width / 2 + 20 &&  x.Position.Y <= Height / 2 + 20 &&
                                        x.Position.X >=   Width / 2 - 20 &&  x.Position.Y >= Height / 2 - 20)));
           
           if(i == 7502){
                WriteAllRobotsToConsole(robots); //print the christmas tree
           }
        }
        /*part1
         var center = new Vector2(Width / 2, Height/ 2);
        var result = robots.Count(x => x.Position.X < center.X && x.Position.Y < center.Y) * 
        robots.Count(x =>x.Position.X < center.X && x.Position.Y > center.Y) *
        robots.Count(x => x.Position.X > center.X && x.Position.Y > center.Y) *
        robots.Count(x =>x.Position.X > center.X && x.Position.Y < center.Y);*/

        
        Console.WriteLine($"Day 14: {results.OrderByDescending(x => x.Item2).First().Item2} ");
    }
    
    private static void WriteAllRobotsToConsole(List<Robot> robots){
        var matrix = new char[Height, Width];
        for(int i = 0; i < Height; i++){
            for(int j = 0; j < Width; j++){
                matrix[i, j] = '.';
            }
        }
        foreach(var robot in robots){
            matrix[(int)robot.Position.Y, (int)robot.Position.X] = '#';
        }
        for(int i = 0; i < Height; i++){
            for(int j = 0; j < Width; j++){
                Console.Write(matrix[i, j]);
            }
            Console.WriteLine();
        }
    }

    private static void MoveRobot(Robot robot){
        robot.Position += robot.Velocity;
        if(robot.Position.X < 0) robot.Position = new Vector2(Width + robot.Position.X, robot.Position.Y);
        if(robot.Position.Y < 0) robot.Position = new Vector2(robot.Position.X, Height + robot.Position.Y);
        if(robot.Position.X >= Width) robot.Position = new Vector2(robot.Position.X - Width, robot.Position.Y);
        if(robot.Position.Y >= Height) robot.Position = new Vector2(robot.Position.X, robot.Position.Y - Height);
    }


}

public class Robot{
    public Vector2 Position {get; set;}  
    public Vector2 Velocity {get; set;}
}
