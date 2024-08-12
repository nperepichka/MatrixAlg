using CubeAlg.Models;
using CubeAlg.Helpers;
using System.Diagnostics;
using CubeAlg.Enums;

namespace CubeAlg;

internal static class Program
{
    private static readonly byte Size = 7;

    private static byte N = 0;

    private static bool ProcessSimetric1 = false;
    private static bool ProcessSimetric2 = false;
    //private static byte IgnoreSimetric2ForX = byte.MaxValue;

    public static readonly HashSet<string> CubeHashes = [];

    private static void Main()
    {
        ProcessSimetric1 = Size % 2 == 1;
        ProcessSimetric2 = !ProcessSimetric1;
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

        Process(ref cube);

        stopwatch.Stop();

        Console.WriteLine($"Unique invariant cubes count: {N}");
        Console.WriteLine();
        Console.WriteLine($"Processing elapsed in {stopwatch.ElapsedMilliseconds * 0.001:0.00}s");

        Console.WriteLine("Done. Press <Enter> to exit...");
        Console.ReadLine();
    }

    private static void Process(ref List<Point> cube)
    {
        var point = cube.FirstOrDefault(p => !p.IsInitialized);

        if (point == null)
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
                if (CubeHashes.Add(hash))
                {
                    N++;
                    Console.WriteLine($"Cube found #{N}:");
                    cube.Print();
                }
            }

            return;
        }

        for (byte y = 0; y < Size; y++)
        {
            var isValid = !cube.Any(p => (p.X == point.X || p.Z == point.Z) && p.Y == y);
            if (isValid)
            {
                point.SetY(y);

                if (ProcessSimetric1)
                {
                    if (y == point.X)
                    {
                        Process(ref cube);
                    }
                    else
                    {
                        var point2 = cube.FirstOrDefault(p => p.Z == point.Z && p.X == y);
                        if (point2?.IsInitialized == false)
                        {
                            point2.SetY(point.X);
                            Process(ref cube);
                            point2.SetY(null);
                        }
                    }
                }

                //if (point.X != IgnoreSimetric2ForX)
                if (ProcessSimetric2)
                {
                    var point3 = cube.FirstOrDefault(p => p.Z == point.Z && p.X == Size - point.X - 1);
                    if (point3?.IsInitialized == false)
                    {
                        point3.SetY((byte)(Size - y - 1));
                        Process(ref cube);
                        point3.SetY(null);
                    }
                }

                point.SetY(null);
            }
        }
    }
}