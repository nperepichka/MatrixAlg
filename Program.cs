using MatrixAlg.Analysers;
using MatrixAlg.Helpers;
using MatrixAlg.Models;
using MatrixAlg.Processors;
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
        // Clear output
        OutputWriter.Clear();
        // Clear mosaics
        MosaicDrawer.Clear();
        // Enable output queue monitoring
        OutputWriter.StartOutputQueueMonitoring();
        // Initiate configuration
        ApplicationConfiguration.Init();

        // Read input matrix
        var input = InputReader.Read();

        // If should analyze cube and input matrix size is more that 8
        if (ApplicationConfiguration.AnalyzeCube && input.GetLength(0) > 8)
        {
            // Write that input matrix size should be 8 or less to enable AnalyzeCube option
            OutputWriter.WriteLine($"Input matrix size should be 8 or less to enable AnalyzeCube option. Application will exit.");
            // Wait for exit confirmed
            WaitForExit();
            //Exit an application
            return;
        }

        // If console output not allowed
        if (!OutputWriter.CanWriteToConsole)
        {
            // Write to console that output will be saved to file
            Console.WriteLine("Processing started. Output will be saved to file.");
        }

        // Start timer to measure running time
        var stopwatch = Stopwatch.StartNew();

        // Write input matrix title
        OutputWriter.WriteLine("Input matrix:");
        // Write input matrix
        DataOutputWriter.WriteMatrix(input);

        // If input matrix is symetric
        if (MatrixSymetricDetector.IsSymetric(input))
        {
            // Write that input matrix is symetric
            OutputWriter.WriteLine("Input matrix is symetric.");
        }
        // Else, if input matrix is self conjugate
        else if (MatrixСonjugationDetector.IsSelfConjugate(input))
        {
            // Write that input matrix is self conjugate
            OutputWriter.WriteLine("Input matrix is self conjugate.");
        }
        // Else, if input matrix is not symetric and is not self conjugate
        else
        {
            // Write that input matrix is not symetric and is not self conjugate
            OutputWriter.WriteLine("Input matrix is not symetric and is not self conjugate.");
        }

        // Find transversal size in input matrix
        var transversal = MatrixTransversalDetector.FindTransversal(input);
        // If transversal exists
        if (transversal > 0)
        {
            // Write that input matrix has transversal
            OutputWriter.WriteLine($"Input matrix has transversal: {transversal}.");
        }
        // Else, if transversal not exists
        else
        {
            // Write that input matrix has no transversal
            OutputWriter.WriteLine($"Input matrix has no transversal. Application will exit.");
            // Wait for exit confirmed
            WaitForExit();
            //Exit an application
            return;
        }

        // Write empty line
        OutputWriter.WriteLine();

        // If should analyze cube
        if (ApplicationConfiguration.AnalyzeCube)
        {
            // Generate combinations for cube analyse
            CubeSymetricDetector.GenerateCombinations(transversal);
        }

        // Initiate decompositor object
        var decompositor = new Decompositor(input, transversal);
        // Process input matrix decomposition on 1-transversals
        decompositor.Decompose();

        // Write empty line
        OutputWriter.WriteLine();
        // Output total decompositions count
        OutputWriter.WriteLine($"Decompositions count: {decompositor.DecomposesCount}");

        // Stop timer
        stopwatch.Stop();

        // If console output not allowed
        if (!OutputWriter.CanWriteToConsole)
        {
            // Write to console that processing id finished
            Console.WriteLine("Processing finished. Pending for output to be saved to file.");
        }

        // Stop output queue monitoring
        OutputWriter.StopOutputQueueMonitoring();

        // Write elapsed time to console
        Console.WriteLine($"Processing elapsed in {stopwatch.ElapsedMilliseconds * 0.001:0.00}s");

        // Wait for exit confirmed
        WaitForExit();
    }

    private static void WaitForExit()
    {
        // Output done message to console
        Console.WriteLine("Done. Press <Enter> to exit...");
        // Wait for "Enter" key
        Console.ReadLine();
    }
}
