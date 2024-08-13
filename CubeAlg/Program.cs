using CubeAlg.Models;
using CubeAlg.Helpers;
using System.Diagnostics;

namespace CubeAlg;

internal static class Program
{
    private static readonly byte Size = 6;

    private static byte N = 0;

    //private static bool ProcessAltS = false;

    private static readonly HashSet<string> CubeHashes = [];
    private static readonly object CubeLock = new();

    private static readonly ParallelOptions ParallelOptions = new() { MaxDegreeOfParallelism = -1 };
    private static readonly int ExpectedParallelsCount = Environment.ProcessorCount / 4;
    private static int ParallelsCount = 0;

    private static void Main()
    {
        Console.WriteLine($"Size: {Size}");
        Console.WriteLine();

        //ProcessAltS = Size % 2 == 0;

        var cube = new List<Point>(Size * Size);

        for (byte z = 0; z < Size; z++)
        {
            for (byte x = 0; x < Size; x++)
            {
                cube.Add(new Point(x, z));
            }
        }

        var stopwatch = Stopwatch.StartNew();

        Process([.. cube]);

        stopwatch.Stop();

        Console.WriteLine($"Unique invariant cubes count: {N}");
        Console.WriteLine();
        Console.WriteLine($"Processing elapsed in {stopwatch.ElapsedMilliseconds * 0.001:0.00}s");

        Console.WriteLine("Done. Press <Enter> to exit...");
        Console.ReadLine();
    }

    private static void Process(Point[] cube)
    {
        for (var i = 0; i < cube.Length; i++)
        {
            if (!cube[i].Y.HasValue)
            {
                if (ParallelsCount >= ExpectedParallelsCount)
                {
                    for (byte y = 0; y < Size; y++)
                    {
                        ProcessItem(cube, i, y);
                    }
                }
                else
                {
                    Interlocked.Add(ref ParallelsCount, Size);
                    Parallel.For(0, Size, ParallelOptions, y =>
                    {
                        ProcessItem(cube, i, (byte)y);
                        Interlocked.Decrement(ref ParallelsCount);
                    });
                }

                return;
            }
        }

        var bCube = cube.BuildCube(Size);
        var topViews = bCube.GetTopViewVariants(Size);

        var rightView = bCube.GetRightView();
        if (topViews[0] != rightView && topViews[1] != rightView && topViews[2] != rightView && topViews[3] != rightView)
        {
            return;
        }

        var backView = bCube.GetBackView();
        if (topViews[0] != backView && topViews[1] != backView && topViews[2] != backView && topViews[3] != backView)
        {
            return;
        }

        var leftView = bCube.GetLeftView();
        if (topViews[0] != leftView && topViews[1] != leftView && topViews[2] != leftView && topViews[3] != leftView)
        {
            return;
        }

        var frontView = bCube.GetFrontView();
        if (topViews[0] != frontView && topViews[1] != frontView && topViews[2] != frontView && topViews[3] != frontView)
        {
            return;
        }

        var hash = topViews.Min()!;
        lock (CubeLock)
        {
            if (CubeHashes.Add(hash))
            {
                N++;
                Console.WriteLine($"Cube found #{N}:");
                cube.PrintCube();
            }
        }
    }

    private static void ProcessItem(Point[] cube, int index, byte y)
    {
        int t;
        for (t = 0; t < cube.Length; t++)
        {
            if (cube[t].Y == y && (cube[t].X == cube[index].X || cube[t].Z == cube[index].Z))
            {
                return;
            }
        }

        var nextCube = cube.CloneCube();
        var nextPoint = nextCube[index];

        nextPoint.SetY(y);

        // ---

        if (y == nextPoint.X)
        {
            Process(nextCube);
            return;
        }

        t = Size * nextPoint.Z + y;
        if (nextCube[t].SetY(nextPoint.X))
        {
            Process(nextCube);
            nextCube[t].SetY(null);
        }

        // maybe if ProcessAltS we can run only this part
        t = Size - y - 1;
        if (nextPoint.X != t && nextCube[Size * (nextPoint.Z + 1) - (nextPoint.X + 1)].SetY((byte)t))
        {
            Process(nextCube);
        }
    }
}