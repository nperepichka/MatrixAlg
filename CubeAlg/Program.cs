using CubeAlg.Helpers;
using CubeAlg.Models;
using MatrixShared.Helpers;
using System.Diagnostics;

namespace CubeAlg;

internal static class Program
{
    private static readonly HashSet<string> Hashes = [];
    private static readonly object Lock = new();

    private static readonly ParallelOptions ParallelOptionsMax = new() { MaxDegreeOfParallelism = ApplicationConfiguration.MaxParallelization };
    private static readonly ParallelOptions ParallelOptions = new() { MaxDegreeOfParallelism = 2 };
    private static int ParallelsCount = 0;

    //private static bool ProcessAltS = false;
    private static byte Size = 0;
    private static byte N = 0;
    private static byte ParallelBeforeIndex = 0;

    private static void Main()
    {
        Size = ConsoleInputReader.ReadSize();

        ParallelBeforeIndex = (byte)(Size - 1);

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

        Process([.. cube], 0);

        stopwatch.Stop();

        Console.WriteLine($"Unique invariant cubes count: {N}");
        Console.WriteLine();
        Console.WriteLine($"Processing elapsed in {stopwatch.ElapsedMilliseconds * 0.001:0.00}s");

        Console.WriteLine("Done. Press <Enter> to exit...");
        Console.ReadLine();
    }

    private static void Process(Point[] cube, int minIndex)
    {
        for (var index = minIndex; index < cube.Length; index++)
        {
            if (!cube[index].Y.HasValue)
            {
                if (ParallelsCount != 0)
                {
                    for (byte y1 = 0; y1 < Size; y1++)
                    {
                        if (ParallelsCount < ApplicationConfiguration.MaxParallelization && y1 < ParallelBeforeIndex)
                        {
                            Interlocked.Increment(ref ParallelsCount);
                            Parallel.For(y1, Size, ParallelOptions, y2 =>
                            {
                                ProcessItem(cube, index, (byte)y2);
                            });
                            Interlocked.Decrement(ref ParallelsCount);
                            return;
                        }

                        ProcessItem(cube, index, y1);
                    }
                }
                else
                {
                    Interlocked.Add(ref ParallelsCount, Size);
                    Parallel.For(0, Size, ParallelOptionsMax, y =>
                    {
                        ProcessItem(cube, index, (byte)y);
                        Interlocked.Decrement(ref ParallelsCount);
                    });
                }
                return;
            }
        }

        var bCube = cube.BuildCube(Size);
        var topViews = bCube.GetTopViewVariants(Size);

        var view = bCube.GetRightView(Size);
        if (topViews[0] != view && topViews[1] != view && topViews[2] != view && topViews[3] != view)
        {
            return;
        }

        view = bCube.GetBackView(Size);
        if (topViews[0] != view && topViews[1] != view && topViews[2] != view && topViews[3] != view)
        {
            return;
        }

        view = bCube.GetLeftView(Size);
        if (topViews[0] != view && topViews[1] != view && topViews[2] != view && topViews[3] != view)
        {
            return;
        }

        view = bCube.GetFrontView(Size);
        if (topViews[0] != view && topViews[1] != view && topViews[2] != view && topViews[3] != view)
        {
            return;
        }

        view = topViews.Min()!;

        Monitor.Enter(Lock);
        if (Hashes.Add(view))
        {
            N++;
            Console.WriteLine($"Cube found #{N}:");
            if (ApplicationConfiguration.OutputAsCoordinates)
            {
                cube.PrintCube();
            }
            else
            {
                view.PrintCubeView(Size);
            }
        }
        Monitor.Exit(Lock);
    }

    private static void ProcessItem(Point[] cube, int index, byte y)
    {
        var p1 = cube[index].Z * Size;
        int p2;

        for (p2 = p1; p2 < p1 + Size; p2++)
        {
            if (cube[p2].Y == y)
            {
                return;
            }
        }
        for (p2 = 0; p2 < Size; p2++)
        {
            if (cube[Size * p2 + cube[index].X].Y == y)
            {
                return;
            }
        }

        var nextCube = cube.CloneCube();
        var nextPoint = nextCube[index];
        index++;

        nextPoint.SetY(y);

        // ---

        if (y == nextPoint.X)
        {
            Process(nextCube, index);
            return;
        }

        p2 = p1 + y;
        if (nextCube[p2].SetY(nextPoint.X))
        {
            Process(nextCube, index);
            nextCube[p2].SetY(null);
        }

        p2 = Size - y - 1;
        if (nextPoint.X != p2 && nextCube[p1 + Size - nextPoint.X - 1].SetY((byte)p2))
        {
            Process(nextCube, index);
        }

        // maybe if ProcessAltS we can run this way (instead of lines after // ---)
        /*p2 = Size - y - 1;
        if (nextPoint.X != p2)
        {
            nextCube[p1 + Size - nextPoint.X - 1].SetY((byte)p2);
        }
        Process(nextCube, index);*/
    }
}