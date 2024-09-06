using MatrixShared.Analysers;
using MatrixShared.Helpers;
using MatrixShared.Models;
using System.Diagnostics;

namespace MatrixDiagAlg;

internal static class Program
{
    private static readonly HashSet<string> Hashes = [];
    private static readonly object Lock = new();

    private static int ParallelsCount = 0;
    private static byte Size = 0;
    private static int MaxLength = 0;
    private static int ParallelBeforeIndex = 0;

    private static readonly List<(byte x, byte y)> Combinations = [];
    //private static readonly List<List<(byte x, byte y)>> Results = [];
    private static int R = 0;

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

        MaxLength = Size + Size - 2;

        for (byte x = 0; x < Size; x++)
        {
            for (byte y = 0; y < Size; y++)
            {
                Combinations.Add((x, y));
            }
        }
        ParallelBeforeIndex = Combinations.Count - 3;

        // Write to console that processing is started
        Console.WriteLine("Will search for diagonal 1-transversals.");
        Console.WriteLine("Processing started.");
        Console.WriteLine();

        // Start timer to measure running time
        var stopwatch = Stopwatch.StartNew();

        Process(0, new byte[Size, Size], 0, false);

        // Stop timer
        stopwatch.Stop();

        Console.WriteLine();
        /*var groupedResults = Results
            .GroupBy(r => r.Count)
            .OrderBy(r => r.Key);
        foreach (var groupedResult in groupedResults)
        {
            Console.WriteLine($"Found {groupedResult.Count()} unique diagonal transversals of {groupedResult.Key} elements:");
            foreach (var res in groupedResult)
            {
                var output = string.Join(" ", res.OrderBy(e => e.x).ThenBy(e => e.y).Select(r => $"{r}"));
                Console.WriteLine(output);
            }
            Console.WriteLine($"{groupedResult.Key} -> {groupedResult.Count()}");
        }*/

        // Write elapsed time to console
        Console.WriteLine($"Processing elapsed in {stopwatch.ElapsedMilliseconds * 0.001:0.00}s");

        // Output done message to console
        Console.WriteLine("Done. Press <Enter> to exit...");
        // Wait for "Enter" key
        Console.ReadLine();
    }

    private static void Process(int startIndex, byte[,] matrix, int count, bool isMatrixFilled)
    {
        if (!isMatrixFilled)
        {
            var nextCount = count + 1;

            for (var index = startIndex; index < Combinations.Count; index++)
            {
                if (ParallelsCount < ParallelDefinitions.ExpectedParallelsCount && index < ParallelBeforeIndex)
                {
                    Parallel.For(index, Combinations.Count, ParallelDefinitions.ParallelOptions, i =>
                    {
                        Interlocked.Increment(ref ParallelsCount);
                        if (matrix[Combinations[i].x, Combinations[i].y] == 0)
                        {
                            TryCloneAndProcess(matrix, nextCount, i);
                        }
                        Interlocked.Decrement(ref ParallelsCount);
                    });
                    break;
                }

                if (matrix[Combinations[index].x, Combinations[index].y] == 0)
                {
                    TryCloneAndProcess(matrix, nextCount, index);
                }
            }
        }
        else if (count >= MaxLength)
        {
            // Build matrix
            var matrixB = new bool[Size, Size];
            for (var i = 0; i < Size; i++)
            {
                for (var j = 0; j < Size; ++j)
                {
                    if (matrix[i, j] == 2)
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
                R++;
                Console.WriteLine($"{DateTime.Now.ToLongTimeString()}: {MaxLength} -> {R}");
            }
            Monitor.Exit(Lock);
        }
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

    private static void TryCloneAndProcess(byte[,] original, int count, int index)
    {
        var isMatrixFilled = true;
        var clone = new byte[Size, Size];
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
                    clone[x, y] = 2;
                }
                else if (y == v1 || y == v2)
                {
                    clone[x, y] = 1;
                }
                else
                {
                    isMatrixFilled = false;
                }
            }
        }

        Process(index + 1, clone, count, isMatrixFilled);
    }
}