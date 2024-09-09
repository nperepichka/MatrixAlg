using AbsolutelySymmetricCubeAlg.Helpers;

namespace AbsolutelySymmetricCubeAlg;

internal static class Program
{
    private static byte Size = 0;

    private static void Main()
    {
        var cube = InputReader.ReadCube();
        Size = (byte)cube.GetLength(0);
        Console.WriteLine($"Size: {Size}");

        while (UpdateCube(cube)) { };

        Console.WriteLine();
        for (var s = 0; s < Size; s++)
        {
            for (var i = 0; i < Size; i++)
            {
                for (var j = 0; j < Size; j++)
                {
                    Console.Write(cube[s, i, j] ? " *" : " O");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }

    public static bool UpdateCube(bool[,,] cube)
    {
        var topView = cube.GetTopView();
        var updated = false;

        // Right
        var index = 0;
        for (var j = Size - 1; j >= 0; j--)
        {
            for (var k = 0; k < Size; k++)
            {
                for (var i = Size - 1; i >= 0; i--)
                {
                    if (topView[index] && !cube[i, j, k])
                    {
                        cube[i, j, k] = true;
                        updated = true;
                    }
                    index++;
                }
            }
        }

        // Back
        index = 0;
        for (var i = 0; i < Size; i++)
        {
            for (var k = 0; k < Size; k++)
            {
                for (var j = Size - 1; j >= 0; j--)
                {
                    if (topView[index] && !cube[i, j, k])
                    {
                        cube[i, j, k] = true;
                        updated = true;
                    }
                    index++;
                }
            }
        }

        // Left
        index = 0;
        for (var j = 0; j < Size; j++)
        {
            for (var k = 0; k < Size; k++)
            {
                for (var i = 0; i < Size; i++)
                {
                    if (topView[index] && !cube[i, j, k])
                    {
                        cube[i, j, k] = true;
                        updated = true;
                    }
                    index++;
                }
            }
        }

        // Front
        index = 0;
        for (var i = Size - 1; i >= 0; i--)
        {
            for (var k = 0; k < Size; k++)
            {
                for (var j = 0; j < Size; j++)
                {
                    if (topView[index] && !cube[i, j, k])
                    {
                        cube[i, j, k] = true;
                        updated = true;
                    }
                    index++;
                }
            }
        }

        return updated;
    }

    private static bool[] GetTopView(this bool[,,] cube)
    {
        var flatArray = new bool[Size * Size * Size];
        var index = 0;

        for (var k = 0; k < Size; k++)
        {
            for (var i = 0; i < Size; i++)
            {
                for (var j = 0; j < Size; j++)
                {
                    flatArray[index] = cube[i, j, k];
                    index++;
                }
            }
        }

        return flatArray;
    }
}