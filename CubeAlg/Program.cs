using CubeAlg.Models;

/// <summary>
/// Application entry point class
/// </summary>
internal static class Program
{
    private static byte Size = 4;

    private static byte MinZ = 0;
    private static byte MaxZ = 0;
    private static byte MinX = 0;
    private static byte MaxX = 0;
    private static byte MinY = 0;
    private static byte MaxY = 0;

    /// <summary>
    /// Application entry point method
    /// </summary>
    private static void Main()
    {
        var fillDiag = Size % 2 == 1;

        MinZ = 0;
        MaxZ = (byte)(fillDiag ? (Size - 1) / 2 : ((Size / 2) - 1));
        MinX = 0;
        MaxX = (byte)(Size - 1);
        MinY = (byte)(fillDiag ? (Size - 1) / 2 : (Size / 2));
        MaxY = (byte)(Size - 1);

        var cube = new List<Point>(Size * Size * Size);

        for (byte z = 0; z < Size; z++)
        {
            for (byte x = 0; x < Size; x++)
            {
                var point = fillDiag && z == x || z == 0 && x == 0
                    ? new Point(x, x, z)
                    : new Point(x, z);
                cube.Add(point);
            }
        }

        Console.WriteLine("Initial state:");
        cube.Print();

        Process(cube);
    }

    private static void Process(List<Point> cube)
    {
        // TODO:
    }

    private static Point? FindNext(this List<Point> cube)
    {
        for (byte z = MinZ; z <= MaxZ; z++)
        {
            for (byte x = MinX; x <= MaxX; x++)
            {
                var point = cube.First(p => p.X == x && p.Z == z);
                if (!point.IsInitialized)
                {
                    return point;
                }
            }
        }
        return null;
    }

    private static void Print(this List<Point> cube)
    {
        foreach (var point in cube)
        {
            point.Print();
        }
        Console.WriteLine();
    }

    private static List<Point> Clone(this List<Point> cube)
    {
        return cube
            .Select(p => p.Clone())
            .ToList();
    }
}