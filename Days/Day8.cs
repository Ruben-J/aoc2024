using System;
using System.Numerics;
using System.Reflection.PortableExecutable;

namespace aoc2024.Days;


public static class Day8
{
    private static char[,] _matrix;
    private static Dictionary<char, List<Vector2>> _antennas = new();
    private static List<Vector2> _antiNodes = new();
    public static async Task Execute()
    {
        _matrix = await ReadMatrixFromFileAsync("Input/Day8.txt");
        RetrieveAntennas();
        RetrieveAntiNodes();
        AddAntennasAsAntiNodes();
        Console.WriteLine($"Day 8: {_antiNodes.Count}");
    }

    private static void AddAntennasAsAntiNodes()
    {
        foreach (var typeAntenna in _antennas)
        {
            foreach (var antenna in typeAntenna.Value)
            {
                AddAntiNode(antenna);
            }
        }
    }

    private static void RetrieveAntiNodes()
    {

        foreach (var typeAntenna in _antennas)
        {
            for (var i = 0; i < typeAntenna.Value.Count; i++)
            {
                for (var j = i + 1; j < typeAntenna.Value.Count; j++)
                {
                    var otherVector = typeAntenna.Value[i] - typeAntenna.Value[j];

                    var antiNode = typeAntenna.Value[j] - otherVector;
                    while (1 == 1)
                    {
                        if (!(antiNode.X >= 0 && antiNode.Y >= 0 && antiNode.X < _matrix.GetLength(0) && antiNode.Y < _matrix.GetLength(1)))
                        {
                            break;
                        }
                        AddAntiNode(antiNode);
                        antiNode -= otherVector;
                    }

                    antiNode = typeAntenna.Value[i] + otherVector;
                    while (1 == 1)
                    {
                        if (!(antiNode.X >= 0 && antiNode.Y >= 0 && antiNode.X < _matrix.GetLength(0) && antiNode.Y < _matrix.GetLength(1)))
                        {
                            break;
                        }
                        AddAntiNode(antiNode);
                        antiNode += otherVector;
                    }

                }
            }

        }
    }

    private static void AddAntiNode(Vector2 antiNode)
    {
        if (!_antiNodes.Contains(antiNode))
        {
            _antiNodes.Add(antiNode);
        }
    }

    private static void RetrieveAntennas()
    {
        for (var i = 0; i < _matrix.GetLength(0); i++)
        {
            for (var j = 0; j < _matrix.GetLength(1); j++)
            {
                if (_matrix[i, j] != '.')
                {
                    if (_antennas.ContainsKey(_matrix[i, j]))
                    {
                        _antennas[_matrix[i, j]].Add(new Vector2(i, j));
                    }
                    else
                    {
                        _antennas.Add(_matrix[i, j], new List<Vector2> { new Vector2(i, j) });
                    }
                }
            }
        }

    }

    private static async Task<char[,]> ReadMatrixFromFileAsync(string filePath)
    {
        var lines = await File.ReadAllLinesAsync(filePath);
        char[,] matrix = new char[lines[0].Length, lines.Length];

        for (var i = 0; i < lines.Length; i++)
        {
            var characters = lines[i].ToCharArray();
            for (var j = 0; j < characters.Length; j++)
            {
                matrix[j, i] = characters[j];
            }
        }
        return matrix;
    }
}
