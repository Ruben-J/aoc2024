using System;
using System.ComponentModel;
using System.Numerics;

namespace aoc2024.Days;

public class Day15
{


    static Vector2 RobotPosition;
    static List<char> Movements = new List<char>();
    private static Vector2 Up = new Vector2(0, -1);
    private static Vector2 Down = new Vector2(0, 1);
    private static Vector2 Right = new Vector2(1, 0);
    private static Vector2 Left = new Vector2(-1, 0);

    private static char Wall = '#';
    private static char Empty = '.';
    private static char Robot = '@';
    private static char BoxLeft = '[';
    private static char BoxRight = ']';
    private static char[,] Matrix;
   
    public static async Task Execute(){
        var lines = await File.ReadAllLinesAsync("Input/Day15.txt");
        Matrix = new char[lines.First().Length * 2, lines.Count(x => x.StartsWith("#"))];
        int lineIndex = 0;
        foreach(var line in lines){
            if(line.StartsWith(Wall)){
            var index =0;
                for(var i = 0; i < line.Length; i++){

                    Matrix[index, lineIndex] = line[i] == 'O' ? '[' : line[i];
                    Matrix[index+1, lineIndex] = line[i] == 'O' ? ']' : line[i] == '@' ? '.' : line[i];
                    if(line[i] == Robot)
                        RobotPosition = new Vector2(i*2, lineIndex);
                    index += 2;
                }
            }
           
            if(line.StartsWith("<") || line.StartsWith(">") || line.StartsWith("^") || line.StartsWith("v")){
                Movements.AddRange(line.ToCharArray());
            }

            lineIndex++;
        }

        foreach(var movement in Movements){
            if(MoveRobot(RobotPosition, GetDirection(movement)))
            {
                MoveRobot(RobotPosition, GetDirection(movement),true);
                Matrix[(int)RobotPosition.X, (int)RobotPosition.Y] = Empty;
                RobotPosition += GetDirection(movement); 
            }
        }

        var total = 0;
        for(int i = 0; i < Matrix.GetLength(1); i++){
            for(int j = 0; j < Matrix.GetLength(0); j++){
               if(Matrix[j, i] == BoxLeft){
                    total += (100 * i) + j;
               }
            }
           
        }
        PrintMatrix();
        Console.WriteLine($"Day 15: {total}");
    }

    private static bool MoveRobot(Vector2 position, Vector2 direction, bool move = false){    
        var newPosition = position + direction;

        if(Matrix[(int)newPosition.X, (int)newPosition.Y] == Wall) return false;
        if(direction.X == -1 || direction.X ==1){
            if(Matrix[(int)newPosition.X, (int)newPosition.Y] == BoxLeft || Matrix[(int)newPosition.X, (int)newPosition.Y]  == BoxRight)
            {
                if(MoveRobot(newPosition, direction)){
                    MoveRobot(newPosition, direction, move);
                    if(move){
                        Matrix[(int)newPosition.X, (int)newPosition.Y] = Matrix[(int)position.X, (int)position.Y];
                        Matrix[(int)position.X, (int)position.Y] = Empty;
                    }
                    return true;
                }
            }
        }
        else if(direction.Y == -1 || direction.Y ==1)
        {
            if(Matrix[(int)newPosition.X, (int)newPosition.Y] == BoxLeft)
            {
                if(MoveRobot(newPosition, direction) && MoveRobot(newPosition + Right, direction))
                {

                    MoveRobot(newPosition, direction,move);
                    MoveRobot(newPosition + Right, direction,move);
                    if(move){
                         Matrix[(int)newPosition.X, (int)newPosition.Y] = Matrix[(int)position.X, (int)position.Y];
                        Matrix[(int)position.X, (int)position.Y] = Empty;
                    }
                    return true;
                }
            }
            if(Matrix[(int)newPosition.X, (int)newPosition.Y] == BoxRight)
            {
                if(MoveRobot(newPosition, direction) && MoveRobot(newPosition + Left, direction))
                {
                    MoveRobot(newPosition, direction,move);
                    MoveRobot(newPosition + Left, direction,move);
                    if(move){
                         Matrix[(int)newPosition.X, (int)newPosition.Y] = Matrix[(int)position.X, (int)position.Y];
                         Matrix[(int)position.X, (int)position.Y] = Empty;

                    }
                    
                    return true;
                }
            }
        }
        if(Matrix[(int)newPosition.X, (int)newPosition.Y] == Empty) {
            if(move){
                Matrix[(int)newPosition.X, (int)newPosition.Y] = Matrix[(int)position.X, (int)position.Y];
                Matrix[(int)position.X, (int)position.Y] = Empty;
            }
            return true;
        }
        
        return false;
    }
    private static Vector2 GetDirection(char direction){
        return direction switch
        {
            '<' => Left,
            '>' => Right,
            '^' => Up,
            'v' => Down,
            _ => throw new InvalidEnumArgumentException()
        };
    }

    private static void PrintMatrix(){
        for(int i = 0; i < Matrix.GetLength(1); i++){
            for(int j = 0; j < Matrix.GetLength(0); j++){
                Console.Write(Matrix[j, i]);
            }
            Console.WriteLine();
        }
    }
}
