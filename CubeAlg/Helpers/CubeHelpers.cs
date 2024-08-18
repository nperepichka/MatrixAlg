using CubeAlg.Models;

namespace CubeAlg.Helpers;

internal static class CubeHelpers
{
    public static void PrintCube(this Point[] cube)
    {
        var sortedCube = cube
            .OrderBy(p => p.X)
            .ThenBy(p => p.Y);
        foreach (var point in sortedCube)
        {
            point.Print();
        }
        Console.WriteLine();
    }

    public static Point[] CloneCube(this Point[] cube)
    {
        var res = new Point[cube.Length];
        for (var i = 0; i < cube.Length; i++)
        {
            res[i] = cube[i].Clone();
        }
        return res;
    }

    public static bool[,,] BuildCube(this Point[] cube, byte size)
    {
        var res = new bool[size, size, size];
        foreach (var point in cube)
        {
            res[point.X, point.Y!.Value, point.Z] = true;
        }
        return res;
    }

    public static string GetTopView(this bool[,,] cube, byte size)
    {
        var flatArray = new char[size * size * size];
        var index = 0;

        for (var k = 0; k < size; k++)
        {
            for (var i = 0; i < size; i++)
            {
                for (var j = 0; j < size; j++)
                {
                    flatArray[index] |= cube[i, j, k] ? '1' : '0';
                    index++;
                }
            }
        }

        return new string(flatArray);
    }

    public static string GetRightView(this bool[,,] cube, byte size)
    {
        var flatArray = new char[size * size * size];
        var index = 0;

        for (var j = size - 1; j >= 0; j--)
        {
            for (var k = 0; k < size; k++)
            {
                for (var i = size - 1; i >= 0; i--)
                {
                    flatArray[index] |= cube[i, j, k] ? '1' : '0';
                    index++;
                }
            }
        }

        return new string(flatArray);
    }

    public static string GetBackView(this bool[,,] cube, byte size)
    {
        var flatArray = new char[size * size * size];
        var index = 0;

        for (var i = 0; i < size; i++)
        {
            for (var k = 0; k < size; k++)
            {
                for (var j = size - 1; j >= 0; j--)
                {
                    flatArray[index] |= cube[i, j, k] ? '1' : '0';
                    index++;
                }
            }
        }

        return new string(flatArray);
    }

    public static string GetLeftView(this bool[,,] cube, byte size)
    {
        var flatArray = new char[size * size * size];
        var index = 0;

        for (var j = 0; j < size; j++)
        {
            for (var k = 0; k < size; k++)
            {
                for (var i = 0; i < size; i++)
                {
                    flatArray[index] |= cube[i, j, k] ? '1' : '0';
                    index++;
                }
            }
        }

        return new string(flatArray);
    }

    public static string GetFrontView(this bool[,,] cube, byte size)
    {
        var flatArray = new char[size * size * size];
        var index = 0;

        for (var i = size - 1; i >= 0; i--)
        {
            for (var k = 0; k < size; k++)
            {
                for (var j = 0; j < size; j++)
                {
                    flatArray[index] |= cube[i, j, k] ? '1' : '0';
                    index++;
                }
            }
        }

        return new string(flatArray);
    }

    public static bool[,,] Rotate(this bool[,,] cube, byte size)
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

    public static List<string> GetTopViewVariants(this bool[,,] cube, byte size)
    {
        var res = new List<string>(4)
        {
            cube.GetTopView(size)
        };

        cube = cube.Rotate(size);
        res.Add(cube.GetTopView(size));

        cube = cube.Rotate(size);
        res.Add(cube.GetTopView(size));

        cube = cube.Rotate(size);
        res.Add(cube.GetTopView(size));

        return res;
    }
}
