using MatrixDiagAlg.Enums;
using MatrixShared.Analysers;
using MatrixShared.Helpers;
using MatrixShared.Models;
using System.Diagnostics;

namespace MatrixDiagAlg;

internal static class Program
{
    private static readonly HashSet<string> Hashes = [];
    private static readonly object Lock = new();

    private static int MaxParallels = ParallelsConfiguration.MaxParallels;
    private static ParallelOptions ParallelOptionsMax = new() { MaxDegreeOfParallelism = MaxParallels };
    private static int ParallelBeforeIndex = 0;
    private static int ParallelsCount = 0;
    private static byte Size = 0;

    private static readonly List<(byte x, byte y)> Combinations = [];
    private static readonly List<bool[,]> Results = [];

    private static void Main()
    {
        var sizeString = string.Empty;
        while (!byte.TryParse(sizeString, out Size) || Size > 250 || Size < 1)
        {
            if (!string.IsNullOrEmpty(sizeString))
            {
                Console.WriteLine("Invalid input.");
            }
            Console.Write("Size: ");
            sizeString = Console.ReadLine();
        }
        Console.WriteLine();

        for (byte x = 0; x < Size; x++)
        {
            for (byte y = 0; y < Size; y++)
            {
                Combinations.Add((x, y));
            }
        }

        if (MaxParallels > Combinations.Count)
        {
            MaxParallels = Combinations.Count;
            ParallelOptionsMax = new() { MaxDegreeOfParallelism = MaxParallels };
        }
        ParallelBeforeIndex = Combinations.Count - MaxParallels;

        // Write to console that processing is started
        Console.WriteLine("Will search for diagonal 1-transversals.");
        Console.WriteLine("Processing started.");
        Console.WriteLine();

        // Start timer to measure running time
        var stopwatch = Stopwatch.StartNew();

        Process(0, new EnumType[Size, Size], 0);

        // Stop timer
        stopwatch.Stop();

        var groupedResults = Results
            .Select(r =>
            {
                var elements = new List<(byte x, byte y)>();
                for (byte x = 0; x < Size; x++)
                {
                    for (byte y = 0; y < Size; ++y)
                    {
                        if (r[x, y])
                        {
                            elements.Add((x, y));
                        }
                    }
                }
                return elements;
            })
            .GroupBy(r => r.Count)
            .OrderBy(r => r.Key);
        foreach (var groupedResult in groupedResults)
        {
            Console.WriteLine();
            Console.WriteLine($"Found {groupedResult.Count()} unique diagonal transversals of {groupedResult.Key} elements:");
            foreach (var res in groupedResult)
            {
                var output = string.Join(" ", res.OrderBy(e => e.x).ThenBy(e => e.y).Select(r => $"{r}"));
                //Console.WriteLine(output);
            }
        }

        // Write elapsed time to console
        Console.WriteLine($"Processing elapsed in {stopwatch.ElapsedMilliseconds * 0.001:0.00}s");

        // Output done message to console
        Console.WriteLine("Done. Press <Enter> to exit...");
        // Wait for "Enter" key
        Console.ReadLine();
    }

    private static void Process(int startIndex, EnumType[,] matrix, int count)
    {
        count++;

        for (var index = startIndex; index < Combinations.Count; index++)
        {
            if (index < ParallelBeforeIndex && ParallelsCount < MaxParallels)
            {
                Parallel.For(index, Combinations.Count, ParallelOptionsMax, i =>
                {
                    Interlocked.Increment(ref ParallelsCount);
                    if (matrix[Combinations[i].x, Combinations[i].y] == 0)
                    {
                        TryCloneAndProcess(matrix, count, i);
                    }
                    Interlocked.Decrement(ref ParallelsCount);
                });
                return;
            }

            if (matrix[Combinations[index].x, Combinations[index].y] == 0)
            {
                TryCloneAndProcess(matrix, count, index);
            }
        }
    }

    private static void ProcessFilled(EnumType[,] matrix)
    {
        // Build matrix
        var matrixB = new bool[Size, Size];
        for (byte i = 0; i < Size; i++)
        {
            for (byte j = 0; j < Size; ++j)
            {
                if (matrix[i, j] == EnumType.Used)
                {
                    matrixB[i, j] = true;
                }
            }
        }

        var minMatrix = matrixB.MirrorMatrixD1(Size).GetMinMatrix(Size, matrixB);
        minMatrix = matrixB.MirrorMatrixD2(Size).GetMinMatrix(Size, minMatrix);
        minMatrix = matrixB.MirrorMatrixH(Size).GetMinMatrix(Size, minMatrix);
        minMatrix = matrixB.MirrorMatrixV(Size).GetMinMatrix(Size, minMatrix);
        matrixB = RotateMatrix(matrixB);
        minMatrix = matrixB.GetMinMatrix(Size, minMatrix);
        matrixB = RotateMatrix(matrixB);
        minMatrix = matrixB.GetMinMatrix(Size, minMatrix);
        matrixB = RotateMatrix(matrixB);
        minMatrix = matrixB.GetMinMatrix(Size, minMatrix);

        var hash = minMatrix.GetHash(Size);

        Monitor.Enter(Lock);
        if (Hashes.Add(hash))
        {
            Results.Add(matrixB);
        }
        Monitor.Exit(Lock);
    }

    private static bool[,] RotateMatrix(bool[,] matrix)
    {
        var res = new bool[Size, Size];

        for (var i = 0; i < Size; i++)
        {
            var ii = Size - i - 1;
            for (var j = 0; j < Size; ++j)
            {
                if (matrix[i, j])
                {
                    res[j, ii] = true;
                }
            }
        }

        return res;
    }

    private static void TryCloneAndProcess(EnumType[,] original, int count, int index)
    {
        var isMatrixFilled = true;
        var clone = new EnumType[Size, Size];

        for (byte x = 0; x < Size; x++)
        {
            var v1 = x - Combinations[index].x + Combinations[index].y;
            var v2 = Combinations[index].x + Combinations[index].y - x;
            for (byte y = 0; y < Size; y++)
            {
                if (original[x, y] != 0)
                {
                    if (y == Combinations[index].y && x == Combinations[index].x)
                    {
                        return;
                    }
                    clone[x, y] = original[x, y];
                }
                else if (x == Combinations[index].x && y == Combinations[index].y)
                {
                    clone[x, y] = EnumType.Used;
                }
                else if (y == v1 || y == v2)
                {
                    clone[x, y] = EnumType.Covered;
                }
                else
                {
                    isMatrixFilled = false;
                }
            }
        }

        if (isMatrixFilled)
        {
            ProcessFilled(clone);
        }
        else
        {
            Process(index + 1, clone, count);
        }
    }
}