using MatrixAlg.Analyzers;
using MatrixAlg.Helpers;
using MatrixAlg.Models;
using MatrixAlg.Processors;
using MatrixShared.Analyzers;
using System.Diagnostics;
using System.Text;

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
        // Enable output queue monitoring
        OutputWriter.StartOutputQueueMonitoring();

        // Read input matrix
        var input = InputReader.Read();

        // Write to console that processing is started
        Console.WriteLine("Processing started.");

        // Start timer to measure running time
        var stopwatch = Stopwatch.StartNew();

        // Write input matrix title
        OutputWriter.WriteLine("Input matrix:");
        // Initiate string builder
        var outputStringBuilder = new StringBuilder(string.Empty);
        // Write input matrix to string builder
        input.WriteMatrix(outputStringBuilder);
        // Write string builder value
        OutputWriter.WriteLine(outputStringBuilder.ToString());

        // If input matrix is symetric
        if (input.IsSymetric())
        {
            // Write that input matrix is symetric
            OutputWriter.WriteLine("Input matrix is symetric.");
        }
        // Else, if input matrix is self conjugate
        else if (input.IsSelfConjugate())
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
        var transversal = input.FindTransversal();
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
            OutputWriter.WriteLine($"Input matrix has no transversal.");
            // Output done message to console
            OutputWriter.WriteLine("Press <Enter> to exit...");
            // Wait for "Enter" key
            Console.ReadLine();
            //Exit an application
            return;
        }

        // Write empty line
        OutputWriter.WriteLine();

        // Initiate decompositor object
        var decompositor = new Decompositor(input, transversal);
        // Process input matrix decomposition on 1-transversals
        decompositor.Decompose();

        // Output total decompositions count
        OutputWriter.WriteLine($"Decompositions count: {decompositor.DecomposesCount}");

        // Stop timer
        stopwatch.Stop();

        // If console output not allowed
        if (!ApplicationConfiguration.EnableConsoleOutput)
        {
            // Write to console that processing id finished
            Console.WriteLine("Processing finished. Pending for output to be saved.");
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
