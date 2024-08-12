using CubeAlg.Enums;
using CubeAlg.Models;
using System.Text;

namespace CubeAlg.Helpers;

internal static class CubeHelpers
{
    public static void Print(this List<Point> cube)
    {
        foreach (var point in cube)
        {
            point.Print();
        }
                Console.WriteLine();
    }

    public static List<Point> Clone(this List<Point> cube)
    {
        return cube
            .Select(p => p.Clone())
            .ToList();
    }

    public static bool[,,] BuildCube(this List<Point> cube, byte size)
    {
        var res = new bool[size, size, size];
        foreach (var point in cube)
        {
            res[point.X, point.Y!.Value, point.Z] = true;
        }
        return res;
    }

    public static string GetView(this bool[,,] cube, CubeView view)
    {
        var size = cube.GetLength(0);
        var size1 = size - 1;
        var stringBuilder = new StringBuilder(string.Empty);

        switch (view)
        {
            case CubeView.Top:
                for (var k = 0; k < size; k++)
                {
                    for (var i = 0; i < size; i++)
                    {
                        for (var j = 0; j < size; j++)
                        {
                            stringBuilder.Append(cube[i, j, k] ? '*' : 'O');
                        }
                    }
                }
                break;
            case CubeView.Right:
                for (var j = size1; j >= 0; j--)
                {
                    for (var k = 0; k < size; k++)
                    {
                        for (var i = size1; i >= 0; i--)
                        {
                            stringBuilder.Append(cube[i, j, k] ? '*' : 'O');
                        }
                    }
                }
                break;
            case CubeView.Back:
                for (var i = 0; i < size; i++)
                {
                    for (var k = 0; k < size; k++)
                    {
                        for (var j = size1; j >= 0; j--)
                        {
                            stringBuilder.Append(cube[i, j, k] ? '*' : 'O');
                        }
                    }
                }
                break;
            case CubeView.Left:
                for (var j = 0; j < size; j++)
                {
                    for (var k = 0; k < size; k++)
                    {
                        for (var i = 0; i < size; i++)
                        {
                            stringBuilder.Append(cube[i, j, k] ? '*' : 'O');
                        }
                    }
                }
                break;
            case CubeView.Front:
                for (var i = size1; i >= 0; i--)
                {
                    for (var k = 0; k < size; k++)
                    {
                        for (var j = 0; j < size; j++)
                        {
                            stringBuilder.Append(cube[i, j, k] ? '*' : 'O');
                        }
                    }
                }
                break;
            case CubeView.Bottom:
                for (var k = size1; k >= 0; k--)
                {
                    for (var i = size1; i >= 0; i--)
                    {
                        for (var j = 0; j < size; j++)
                        {
                            stringBuilder.Append(cube[i, j, k] ? '*' : 'O');
                        }
                    }
                }
                break;
        }

        return stringBuilder.ToString();
    }

    public static bool[,,] Rotate(this bool[,,] cube, int size)
    {
        var res = new bool[size, size, size];

        for (var z = 0; z < size; z++)
        {
            for (var x = 0; x < size; x++)
            {
                for (var y = 0; y < size; ++y)
                {
                    if (cube[x, y, z])
                    {
                        res[y, size - x - 1, z] = true;
                        break;
                    }
                }
            }
        }

        return res;
    }

    public static List<string> GetViewVariants(this bool[,,] cube, CubeView view, int size)
    {
        var res = new List<string>(4)
        {
            cube.GetView(view)
        };

        cube = cube.Rotate(size);
        res.Add(GetView(cube, view));

        cube = cube.Rotate(size);
        res.Add(GetView(cube, view));

        cube = cube.Rotate(size);
        res.Add(GetView(cube, view));

        return res;
    }
}
