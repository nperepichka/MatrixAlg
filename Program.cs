using MatrixAlg.Analysers;
using MatrixAlg.Helpers;
using System.Diagnostics;

namespace MatrixAlg;

/// <summary>
/// Application entry point class
/// </summary>
internal class Program
{
    /// <summary>
    /// Decomposition counter
    /// </summary>
    private static long DecomposeCount = 0;

    private static bool ShowOutput = false;

    /// <summary>
    /// Application entry point method
    /// </summary>
    private static async Task Main(string[] args)
    {
        if (args.Length > 0)
        {
            if (args.Contains("show"))
            {
                ShowOutput = true;
            }
        }

        // Start timer to measure running time
        var stopwatch = Stopwatch.StartNew();

        // Read input matrix
        var input = InputReader.Read();

        // Write to console input matrix title
        Console.WriteLine("Input matrix:");
        // Output input matrix to console
        ConsoleWriter.Write(input);

        // Check if input matrix is symetric
        var isInputSymetric = SymetricDetector.IsSymetric(input);
        // If is symetric
        if (isInputSymetric)
        {
            // Write to console that matrix is symetric
            Console.WriteLine("Input matrix is symetric.");
        }
        // If is not symetric
        else
        {
            // Write to console that matrix is symetric
            Console.WriteLine("Input matrix is not symetric.");
        }

        // Find transversal size in input matrix
        var transversal = TransversalDetector.FindTransversal(input);
        // If transversal exists
        if (transversal > 0)
        {
            // Write to console that matrix has transversal
            Console.WriteLine($"Input matrix has transversal: {transversal}.");
        }
        // If transversal not exists
        else
        {
            // Write to console that matrix has no transversal
            Console.WriteLine($"Input matrix has no transversal.");
            //Exit an application
            return;
        }

        // Write empty line to console
        Console.WriteLine();

        // Initiate decompositor
        using var decompositor = new Decompositor(input, transversal, WriteDecomposeOutput);
        // Process input matrix decomposition on 1-transversals
        await decompositor.Decompose();

        // Write empty line to console
        Console.WriteLine();
        // Output total decompositions count value to console
        Console.WriteLine($"Decompositions count: {DecomposeCount}");

        // Stop timer
        stopwatch.Stop();
        // Write elapsed time to console
        Console.WriteLine($"Elapsed: {stopwatch.ElapsedMilliseconds * 0.001:0.00}s");

        // Get current process
        using var proc = Process.GetCurrentProcess();
        // Measure memory usage
        var memory = proc.PrivateMemorySize64 / (1024 * 1024);
        // Output memory usage to console
        Console.WriteLine($"Memory used: {memory} Mb");

        // Output done message to console
        Console.WriteLine("Done. Press <Enter> to exit...");
        // Wait for "Enter" key
        Console.ReadLine();
    }

    /// <summary>
    /// Write decomposition output to console
    /// </summary>
    /// <param name="res">List of decompositions on 1-transversals</param>
    private static void WriteDecomposeOutput(IEnumerable<int[]> res)
    {
        // Increment decomposition counter
        Interlocked.Increment(ref DecomposeCount);

        if (!ShowOutput)
        {
            return;
        }

        // Output decomposition counter value to console
        Console.WriteLine($"Decomposition #{DecomposeCount}");

        // Define a list to store all not symetric matrixes with indexes
        var notSymetricMatrixes = new List<(bool[,] matrix, int index)>();

        // Build and enumerate all matrixes
        foreach (var matrix in res.Select(BuildMatrix))
        {
            // Check if matrix is symetric
            var isSymetric = SymetricDetector.IsSymetric(matrix);
            // If is symetric
            if (isSymetric)
            {
                // Output matrix to console
                ConsoleWriter.Write(matrix);
                // Write to console that matrix is symetric
                Console.WriteLine("Matrix is symetric.");
            }
            // If is not symetric
            else
            {
                // Add matrix to list of all not symetric matrixes
                notSymetricMatrixes.Add((matrix, notSymetricMatrixes.Count));
            }
        }

        // Enumerate all not symetric matrixes
        foreach (var (matrix, index) in notSymetricMatrixes)
        {
            // Output matrix to console
            ConsoleWriter.Write(matrix);
            // Write to console that matrix is not symetric
            Console.WriteLine("Matrix is not symetric.");

            // Check if similar not symetric matrix exists
            var similarExists = notSymetricMatrixes.Any(m => m.index != index && SymetricDetector.AreSimilar(matrix, m.matrix));
            // If similar not symetric matrix exists
            if (similarExists)
            {
                // Write to console that matrix is similar to other not symetric matrix
                Console.WriteLine("Matrix is similar to other not symetric matrix.");
            }
            // If similar not symetric matrix not exists
            else
            {
                // Write to console that matrix is not similar to other not symetric matrix
                Console.WriteLine("Matrix is not similar to other not symetric matrix.");
            }
        }

        // Write empty line to console
        Console.WriteLine();
    }

    private static bool[,] BuildMatrix(int[] indexes)
    {
        var size = indexes.Length;
        var res = new bool[size, size];
        for (var i = 0; i < size; i++)
        {
            for (var j = 0; j < size; j++)
            {
                res[i, j] = indexes[i] == j;
            }
        }
        return res;
    }
}
