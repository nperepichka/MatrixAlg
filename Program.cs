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
        // Enable output queue monitoring
        OutputWriter.StartOutputQueueMonitoring();

        // Read input matrix
        var input = InputReader.Read();

        // If console output not allowed
        if (!OutputWriter.CanWriteToConsole)
        {
            // Write that output will be saved to file
            Console.WriteLine("Processing started. Output will be saved to file.");
        }

        // Write input matrix title
        OutputWriter.WriteLine("Input matrix:");
        // Write input matrix
        DataOutputWriter.WriteMatrix(input);

        // If input matrix is symetric
        if (MatrixSymetricDetector.IsSymetric(input))
        {
            // Write that matrix is symetric
            OutputWriter.WriteLine("Input matrix is symetric.");
        }
        // If input matrix is self conjugate
        else if (MatrixСonjugationDetector.IsSelfConjugate(input))
        {
            // Write that matrix is self conjugate
            OutputWriter.WriteLine("Input matrix is self conjugate.");
        }
        // If input matrix is not symetric and is not self conjugate
        else
        {
            // Write that matrix is not symetric and is not self conjugate
            OutputWriter.WriteLine("Input matrix is not symetric and is not self conjugate.");
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

        // Generate combinations for cube analyse
        CubeSymetricDetector.GenerateCombinations(transversal);

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

        // If console output not allowed
        if (!OutputWriter.CanWriteToConsole)
        {
            // Write that processing id finished
            Console.WriteLine("Processing finished. Pending for output to be saved to file.");
        }

        // Stop output queue monitoring
        OutputWriter.StopOutputQueueMonitoring();

        // Write elapsed time to console
        Console.WriteLine($"Processing elapsed in {stopwatch.ElapsedMilliseconds * 0.001:0.00}s");

        // Output done message to console
        Console.WriteLine("Done. Press <Enter> to exit...");
        // Wait for "Enter" key
        Console.ReadLine();
    }
}
