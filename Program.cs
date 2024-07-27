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
    private static void Main()
    {
        // Start timer to measure running time
        var stopwatch = Stopwatch.StartNew();

        // Clear output
        OutputWriter.Clear();

        // Read input matrix
        var input = InputReader.Read();

        // Write input matrix title
        OutputWriter.WriteLine("Input matrix:");
        // Write input matrix
        OutputWriter.WriteMatrix(input);

        // If input matrix is symetric
        if (MatrixSymetricDetector.IsSymetric(input))
        {
            // Write that matrix is symetric
            OutputWriter.WriteLine("Input matrix is symetric.");
        }
        // If input matrix is self similar
        else if (MatrixSimilarDetector.IsSelfSimilar(input))
        {
            // Write that matrix is self similar
            OutputWriter.WriteLine("Input matrix is self similar.");
        }
        // If input matrix is not symetric and is not self similar
        else
        {
            // Write that matrix is not symetric and is not self similar
            OutputWriter.WriteLine("Input matrix is not symetric and is not self similar.");
        }

        // Find transversal size in input matrix
        var transversal = MatrixTransversalDetector.FindTransversal(input);
        // If transversal exists
        if (transversal > 0)
        {
            // Write that matrix has transversal
            OutputWriter.WriteLine($"Input matrix has transversal: {transversal}.");
        }
        // If transversal not exists
        else
        {
            // Write that matrix has no transversal
            OutputWriter.WriteLine($"Input matrix has no transversal.");
            //Exit an application
            return;
        }

        // Write empty line
        OutputWriter.WriteLine();

        // Initiate decompositor
        var decompositor = new Decompositor(input, transversal);
        // Process input matrix decomposition on 1-transversals
        decompositor.Decompose();

        // Write empty line
        OutputWriter.WriteLine();
        // Output total decompositions count value
        OutputWriter.WriteLine($"Decompositions count: {decompositor.DecomposesCount}");

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
