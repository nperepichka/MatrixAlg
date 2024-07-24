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
    /// Application entry point method
    /// </summary>
    private static void Main(string[] args)
    {
        // Start timer to measure running time
        var stopwatch = Stopwatch.StartNew();

        // Read input matrix
        var input = InputReader.Read();

        // Write to console input matrix title
        Console.WriteLine("Input matrix:");
        // Output input matrix to console
        ConsoleWriter.WriteMatrix(input);

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

        // Detect if show output param passed in console
        var showOutput = args?.Contains("show") == true;

        // Initiate decompositor
        var decompositor = new Decompositor(input, transversal, showOutput);
        // Process input matrix decomposition on 1-transversals
        decompositor.Decompose();

        // Write empty line to console
        Console.WriteLine();
        // Output total decompositions count value to console
        Console.WriteLine($"Decompositions count: {decompositor.DecomposesCount}");

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
}
