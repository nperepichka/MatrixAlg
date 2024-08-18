using MatrixShared.Analysers;
using MatrixShared.Helpers;
using System.Diagnostics;

namespace MatrixDiagAlg;

internal static class Program
{
    private static byte Size;
    private static readonly List<List<(byte x, byte y)>> Results = [];
    private static readonly HashSet<string> Hashes = [];
    //private static int max = 0;

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

                        var newMatrix = CloneMatrix(elements, matrix, x, y);

                        if (newMatrix != null)
                        {
                            var newElements = elements.ToList();
                            newElements.Add((x, y));

                            Process(newElements, newMatrix);
                        }
                    }
                }
            }
        }

        if (isMatrixFilled /*&& elements.Count >= max*/)
        {
            /*if (elements.Count > max)
            {
                Hashes.Clear();
            }
            max = elements.Count;*/
            var hashes = new string[8];

            matrix = BuildMatrix(elements);
            hashes[0] = matrix.GetHash();

            hashes[1] = matrix.MirrorMatrixD1().GetHash();
            hashes[2] = matrix.MirrorMatrixD2().GetHash();
            hashes[3] = matrix.MirrorMatrixH().GetHash();
            hashes[4] = matrix.MirrorMatrixV().GetHash();

            var matrix2 = RotateMatrix(matrix);
            hashes[5] = matrix2.GetHash();

            matrix2 = RotateMatrix(matrix2);
            hashes[6] = matrix2.GetHash();

            matrix2 = RotateMatrix(matrix2);
            hashes[7] = matrix2.GetHash();

            /*matrix = BuildMatrix(elements);
            hashes[0] = matrix.GetHash();

            hashes[1] = matrix.MirrorMatrixD1().GetHash();
            hashes[2] = matrix.MirrorMatrixD2().GetHash();
            hashes[3] = matrix.MirrorMatrixH().GetHash();
            hashes[4] = matrix.MirrorMatrixV().GetHash();

            var matrix2 = RotateMatrix(matrix);
            hashes[5] = matrix2.GetHash();

            hashes[6] = matrix2.MirrorMatrixD1().GetHash();
            hashes[7] = matrix2.MirrorMatrixD2().GetHash();
            hashes[8] = matrix2.MirrorMatrixH().GetHash();
            hashes[9] = matrix2.MirrorMatrixV().GetHash();

            matrix2 = RotateMatrix(matrix2);
            hashes[10] = matrix2.GetHash();

            hashes[11] = matrix2.MirrorMatrixD1().GetHash();
            hashes[12] = matrix2.MirrorMatrixD2().GetHash();
            hashes[13] = matrix2.MirrorMatrixH().GetHash();
            hashes[14] = matrix2.MirrorMatrixV().GetHash();

            matrix2 = RotateMatrix(matrix2);
            hashes[15] = matrix2.GetHash();

            hashes[16] = matrix2.MirrorMatrixD1().GetHash();
            hashes[17] = matrix2.MirrorMatrixD2().GetHash();
            hashes[18] = matrix2.MirrorMatrixH().GetHash();
            hashes[19] = matrix2.MirrorMatrixV().GetHash();*/

            var hash = hashes.Min()!;
            if (Hashes.Add(hash))
            {
                Results.Add(elements);
            }
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

    private static bool[,]? CloneMatrix(List<(byte x, byte y)> elements, bool[,] original, byte newX, byte newY)
    {
        var clone = new bool[Size, Size];
        var v1 = newX - newY;
        var v2 = newX + newY;

        for (byte x = 0; x < Size; x++)
        {
            for (byte y = 0; y < Size; y++)
            {
                if (original[x, y])
                {
                    clone[x, y] = true;
                }
                else if (x - y == v1 || x + y == v2)
                {
                    for (var i = 0; i < elements.Count; i++)
                    {
                        if (elements[i].x == x && elements[i].y == y)
                        {
                            return null;
                        }
                    }
                    clone[x, y] = true;
                }
            }
        }

        return clone;
    }

    public static bool[,] BuildMatrix(List<(byte x, byte y)> elements)
    {
        var res = new bool[Size, Size];
        foreach (var (x, y) in elements)
        {
            res[x, y] = true;
        }
        return res;
    }

}