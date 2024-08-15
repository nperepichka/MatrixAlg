using MosaicAlg.Helpers;
using MosaicAlg.Processors;
using System.Diagnostics;

namespace MosaicAlg;

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
        byte size;
        var sizeString = string.Empty;
        while (!byte.TryParse(sizeString, out size) || size > 250 || size < 1)
        {
            if (!string.IsNullOrEmpty(sizeString))
            {
                Console.WriteLine("Invalid input.");
            }
            Console.Write("Size: ");
            sizeString = Console.ReadLine();
        }
        Console.WriteLine();

        // Write to console that cleaning output is started
        Console.WriteLine("Cleaning output started.");
        // Clear mosaics
        MosaicDrawer.Clear();

        // Write to console that processing is started
        Console.WriteLine("Processing started.");
        Console.WriteLine();

        // Start timer to measure running time
        var stopwatch = Stopwatch.StartNew();

        // Initiate decompositor object
        var decompositor = new Decompositor(size);
        // Process input matrix decomposition on 1-transversals
        decompositor.Decompose();

        // Stop timer
        stopwatch.Stop();

        // Write elapsed time to console
        Console.WriteLine($"Processing elapsed in {stopwatch.ElapsedMilliseconds * 0.001:0.00}s");

        // Output done message to console
        Console.WriteLine("Done. Press <Enter> to exit...");
        // Wait for "Enter" key
        Console.ReadLine();
    }
}
