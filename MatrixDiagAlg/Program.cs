using System.Diagnostics;

namespace MatrixDiagAlg;

internal static class Program
{
    private static byte Size;
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
            .GroupBy(r => r.Count);
        foreach (var groupedResult in groupedResults)
        {
            Console.WriteLine($"Diagonal transversals of {groupedResult.Key} elements: {groupedResult.Count()}");
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
        (byte? lastX, byte? lastY) = (null, null);
        if (elements.Count > 0)
        {
            (lastX, lastY) = elements[^1];
        }
        var canTake = lastX == null;

        for (byte x = 0; x < Size; x++)
        {
            for (byte y = 0; y < Size; y++)
            {
                if (!matrix[x, y])
                {
                    isMatrixFilled = false;
                    if (canTake || x > lastX!.Value || x == lastX!.Value && y > lastY!.Value)
                    {
                        canTake = true;

                        var newElements = elements.ToList();
                        newElements.Add((x, y));

                        var newMatrix = CloneMatrix(matrix, x, y);

                        Process(newElements, newMatrix);
                    }
                }
            }
        }

        if (isMatrixFilled)
        {
            Console.WriteLine($"Diagonal transversal found ({elements.Count}):");
            var sortedElements = elements
                .OrderBy(e => e.x)
                .ThenBy(e => e.y)
                .ToList();
            foreach (var (x, y) in sortedElements)
            {
                Console.WriteLine($"({x}, {y})");
            }
            Results.Add(sortedElements);
        }
    }

    private static bool[,] CloneMatrix(bool[,] original, byte newX, byte newY)
    {
        var clone = new bool[Size, Size];
        var v1 = (byte)(newX - newY);
        var v2 = (byte)(newX + newY);

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