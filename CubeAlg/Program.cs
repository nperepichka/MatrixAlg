using CubeAlg.Models;
using CubeAlg.Helpers;
using System.Diagnostics;
using CubeAlg.Enums;

namespace CubeAlg;

internal static class Program
{
    private static readonly byte Size = 6;

    private static byte N = 0;

    private static bool ProcessSimetric1 = false;
    //private static bool ProcessSimetric2 = false;
    //private static byte IgnoreSimetric2ForX = byte.MaxValue;

    private static readonly HashSet<string> CubeHashes = [];
    private static readonly object CubeLock = new();

    private static readonly ParallelOptions ParallelOptions = new() { MaxDegreeOfParallelism = -1 };

    private static void Main()
    {
        ProcessSimetric1 = Size % 2 == 1;
        //ProcessSimetric2 = !ProcessSimetric1;
        /*if (ProcessSimetric1)
        {
            IgnoreSimetric2ForX = (byte) ((Size + 1) / 2);
        }*/

        Console.WriteLine($"Size: {Size}");
        Console.WriteLine();

        var cube = new List<Point>(Size * Size);

        for (byte z = 0; z < Size; z++)
        {
            for (byte x = 0; x < Size; x++)
            {
                cube.Add(new Point(x, z));
            }
        }

        var stopwatch = Stopwatch.StartNew();

        Process([.. cube], true);

        stopwatch.Stop();

        Console.WriteLine($"Unique invariant cubes count: {N}");
        Console.WriteLine();
        Console.WriteLine($"Processing elapsed in {stopwatch.ElapsedMilliseconds * 0.001:0.00}s");

        Console.WriteLine("Done. Press <Enter> to exit...");
        Console.ReadLine();
    }

    private static void Process(Point[] cube, bool runParallel = false)
    {
        int? index = null;
        for (int i = 0; i < cube.Length; i++)
        {
            if (!cube[i].IsInitialized)
            {
                index = i;
                break;
            }
        }

        if (!index.HasValue)
        {
            var bCube = cube.BuildCube(Size);
            var topViews = bCube.GetViewVariants(CubeView.Top, Size);

            if (
                   topViews.Any(tv => tv == bCube.GetView(CubeView.Right))
                && topViews.Any(tv => tv == bCube.GetView(CubeView.Back))
                && topViews.Any(tv => tv == bCube.GetView(CubeView.Left))
                && topViews.Any(tv => tv == bCube.GetView(CubeView.Front))
                )
            {
                var hash = topViews.Min(tv => tv)!;
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

            return;
        }

        if (runParallel)
        {
            Parallel.For(0, Size, ParallelOptions, y =>
            {
                ProcessItem(cube, index.Value, (byte)y);
            });
        }
        else
        {
            for (byte y = 0; y < Size; y++)
            {
                ProcessItem(cube, index.Value, y);
            }
        }
    }

    private static void ProcessItem(Point[] cube, int index, byte y)
    {
        if (cube.Any(p => (p.X == cube[index].X || p.Z == cube[index].Z) && p.Y == y))
        {
            return;
        }

        var nextCube = cube.CloneCube();
        var nextPoint = nextCube[index];

        nextPoint.SetY(y);

        if (ProcessSimetric1)
        {
            if (y == nextPoint.X)
            {
                Process(nextCube);
            }
            else
            {
                var point2 = nextCube.FirstOrDefault(p => p.Z == nextPoint.Z && p.X == y);
                if (point2?.IsInitialized == false)
                {
                    point2.SetY(nextPoint.X);
                    Process(nextCube);
                }
            }
        }
        else
        {
            var point3 = nextCube.FirstOrDefault(p => p.Z == nextPoint.Z && p.X == Size - nextPoint.X - 1);
            if (point3?.IsInitialized == false)
            {
                point3.SetY((byte)(Size - y - 1));
                Process(nextCube);
            }
        }
    }
}