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
        ParallelBeforeIndex = Combinations.Count - 3;

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
        var canProcess = true;
        var isMatrixFilled = true;
        byte lastX = 0;
        byte lastY = 0;
        if (elements.Count != 0)
        {
            canProcess = false;
            (lastX, lastY) = elements[^1];
        }

        void processXY(byte x, byte y)
        {
            if (!matrix[x, y])
            {
                isMatrixFilled = false;
                if (canProcess || x > lastX || x == lastX && y > lastY)
                {
                    canProcess = true;
                    var newMatrix = TryCloneMatrix(elements, matrix, x, y);
                    if (newMatrix != null)
                    {
                        var newElements = new List<(byte x, byte y)>(elements)
                        {
                            (x, y)
                        };
                        Process(newElements, newMatrix);
                    }
                }
            }
        }

        for (var index = 0; index < Combinations.Count; index++)
        {
            if (ParallelsCount < ParallelDefinitions.ExpectedParallelsCount && index < ParallelBeforeIndex)
            {
                Parallel.For(index, Combinations.Count, ParallelDefinitions.ParallelOptions, i =>
                {
                    Interlocked.Increment(ref ParallelsCount);
                    processXY(Combinations[i].x, Combinations[i].y);
                    Interlocked.Decrement(ref ParallelsCount);
                });
                break;
            }
            processXY(Combinations[index].x, Combinations[index].y);
        }

        if (isMatrixFilled && elements.Count >= MaxLength)
        {
            // Build matrix
            matrix = new bool[Size, Size];
            for (var i = 0; i < elements.Count; i++)
            {
                matrix[elements[i].x, elements[i].y] = true;
            }

            var minMatrix = matrix.MirrorMatrixD1(Size).GetMinMatrix(Size, matrix);
            minMatrix = matrix.MirrorMatrixD2(Size).GetMinMatrix(Size, minMatrix);
            minMatrix = matrix.MirrorMatrixH(Size).GetMinMatrix(Size, minMatrix);
            minMatrix = matrix.MirrorMatrixV(Size).GetMinMatrix(Size, minMatrix);
            matrix = RotateMatrix(matrix);
            minMatrix = matrix.GetMinMatrix(Size, minMatrix);
            matrix = RotateMatrix(matrix);
            minMatrix = matrix.GetMinMatrix(Size, minMatrix);
            matrix = RotateMatrix(matrix);
            minMatrix = matrix.GetMinMatrix(Size, minMatrix);

            var hash = minMatrix.GetHash(Size);

            Monitor.Enter(Lock);
            if (!Hashes.Add(hash))
            {
                Monitor.Exit(Lock);
                return;
            }

            Results.Add(elements);
            Monitor.Exit(Lock);

            Console.WriteLine($"{DateTime.Now.ToLongTimeString()}: {MaxLength} -> {Results.Count}");
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
            v1 = x - newX + newY;
            v2 = newX + newY - x;
            for (byte y = 0; y < Size; y++)
            {
                if (original[x, y] || y == v1 || y == v2)
                {
                    clone[x, y] = true;
                }
            }
        }

        return clone;
    }
}