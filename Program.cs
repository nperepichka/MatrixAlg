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
        // Write to console that cleaning output is started
        Console.WriteLine("Cleaning output started.");
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
        // Get size of input matrix
        var size = (byte)input.GetLength(0);

        // If should analyze cubes and input matrix size is more that 7
        if (ApplicationConfiguration.AnalyzeCubes && size > 7)
        {
            // Write that input matrix size should be 7 or less to enable AnalyzeCube option
            OutputWriter.WriteLine($"Input matrix size should be 7 or less to enable AnalyzeCube option. Application will exit.");
            // Wait for exit confirmed
            WaitForExit();
            //Exit an application
            return;
        }

        // Write to console that processing is started
        Console.WriteLine("Processing started.");

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

        // If should analyze cubes
        if (ApplicationConfiguration.AnalyzeCubes)
        {
            if (transversal != size)
            {
                OutputWriter.WriteLine($"Input matrix size should be equal to amount of transversals to enable AnalyzeCube option. Application will exit.");
                // Wait for exit confirmed
                WaitForExit();
                //Exit an application
                return;
            }
            // Generate combinations for cube analyse
            CubeCreator.GenerateCombinations(size);
        }

        // Initiate cube creator object
        var cubeCreator = new CubeCreator(transversal);
        // Initiate decompositor object
        var decompositor = new Decompositor(input, transversal, cubeCreator);
        // Process input matrix decomposition on 1-transversals
        decompositor.Decompose();

        cubeCreator.OutputCubes();

        // Write empty line
        OutputWriter.WriteLine();
        // Output total decompositions count
        OutputWriter.WriteLine($"Decompositions count: {decompositor.DecomposesCount}");
        if (ApplicationConfiguration.AnalyzeCubes)
        {
            OutputWriter.WriteLine($"Unique invariant cubes count: {cubeCreator.CubeViews.Count}");
        }

        // Stop timer
        stopwatch.Stop();

        // If console output not allowed
        if (!OutputWriter.CanWriteToConsole)
        {
            // Write to console that processing id finished
            Console.WriteLine("Processing finished. Pending for output to be saved.");
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
