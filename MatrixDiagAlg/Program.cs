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
    private static byte Size;
    private static int MaxLength = 0;

    private static readonly List<(byte x, byte y)> Combinations = [];
    private static readonly List<List<(byte x, byte y)>> Results = [];

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

        // Write to console that processing is started
        Console.WriteLine("Will search for diagonal 1-transversals.");
        Console.WriteLine("Processing started.");
        Console.WriteLine();

        // Start timer to measure running time
        var stopwatch = Stopwatch.StartNew();

        Process([], new bool[Size, Size]);

        // Stop timer
        stopwatch.Stop();

        Console.WriteLine();
        var groupedResults = Results
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
        }

        // Write elapsed time to console
        Console.WriteLine($"Processing elapsed in {stopwatch.ElapsedMilliseconds * 0.001:0.00}s");

        // Output done message to console
        Console.WriteLine("Done. Press <Enter> to exit...");
        // Wait for "Enter" key
        Console.ReadLine();
    }

    private static void Process(List<(byte x, byte y)> elements, bool[,] matrix)
    {
        var isMatrixFilled = true;
        var (lastX, lastY) = elements.Count != 0
            ? elements[^1]
            : Combinations[0];

        void processXY(byte x, byte y)
        {
            if (!matrix[x, y])
            {
                isMatrixFilled = false;
                if (x > lastX || x == lastX && y > lastY)
                {
                    var newMatrix = TryCloneMatrix(elements, matrix, x, y);
                    if (newMatrix != null)
                    {
                        var newElements = elements.ToList();
                        newElements.Add((x, y));
                        Process(newElements, newMatrix);
                    }
                }
            }
        }

        if (ParallelsCount >= ParallelDefinitions.ExpectedParallelsCount)
        {
            for (var i = 0; i < Combinations.Count; i++)
            {
                processXY(Combinations[i].x, Combinations[i].y);
            }
        }
        else
        {
            Parallel.For(0, Combinations.Count, ParallelDefinitions.ParallelOptions, i =>
            {
                Interlocked.Increment(ref ParallelsCount);
                processXY(Combinations[i].x, Combinations[i].y);
                Interlocked.Decrement(ref ParallelsCount);
            });
        }

        if (isMatrixFilled && elements.Count >= MaxLength)
        {
            // Build matrix
            matrix = new bool[Size, Size];
            for (var i = 0; i < elements.Count; i++)
            {
                matrix[elements[i].x, elements[i].y] = true;
            }

            var hash = matrix.GetHash(Size);

            Monitor.Enter(Lock);
            if (Hashes.Add(hash))
            {
                Monitor.Exit(Lock);
                Console.WriteLine($"{DateTime.Now.ToLongTimeString()}: {MaxLength} -> {Results.Count}");

                var hash1 = matrix.MirrorMatrixD1(Size).GetHash(Size);
                var hash2 = matrix.MirrorMatrixD2(Size).GetHash(Size);
                var hash3 = matrix.MirrorMatrixH(Size).GetHash(Size);
                var hash4 = matrix.MirrorMatrixV(Size).GetHash(Size);
                matrix = RotateMatrix(matrix);
                var hash5 = matrix.GetHash(Size);
                matrix = RotateMatrix(matrix);
                var hash6 = matrix.GetHash(Size);
                matrix = RotateMatrix(matrix);
                var hash7 = matrix.GetHash(Size);

                Monitor.Enter(Lock);
                Results.Add(elements);
                Hashes.Add(hash1);
                Hashes.Add(hash2);
                Hashes.Add(hash3);
                Hashes.Add(hash4);
                Hashes.Add(hash5);
                Hashes.Add(hash6);
                Hashes.Add(hash7);
            }
            Monitor.Exit(Lock);
        }
    }

    private static bool[,] RotateMatrix(bool[,] matrix)
    {
        var res = new bool[Size, Size];

        for (var i = 0; i < Size; i++)
        {
            for (var j = 0; j < Size; ++j)
            {
                if (matrix[i, j])
                {
                    res[j, Size - i - 1] = true;
                }
            }
        }

        return res;
    }

    private static bool[,]? TryCloneMatrix(List<(byte x, byte y)> elements, bool[,] original, byte newX, byte newY)
    {
        var v1 = newX - newY;
        var v2 = newX + newY;

        for (var i = 0; i < elements.Count; i++)
        {
            if (elements[i].x - elements[i].y == v1 || elements[i].x + elements[i].y == v2)
            {
                return null;
            }
        }

        var clone = new bool[Size, Size];
        for (byte x = 0; x < Size; x++)
        {
            for (byte y = 0; y < Size; y++)
            {
                if (original[x, y] || x - y == v1 || x + y == v2)
                {
                    clone[x, y] = true;
                }
            }
        }

        return clone;
    }
}