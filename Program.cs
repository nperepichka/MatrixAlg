using MatrixAlg.Analysers;
using MatrixAlg.Helpers;
using System.Diagnostics;

internal class Program
{
    private static void Main()
    {
        var stopwatch = Stopwatch.StartNew();

        var input = InputReader.Read();

        Console.WriteLine("Input matrix:");
        ConsoleWriter.Write(input);

        var size = input.GetLength(0);
        if (size < 2)
        {
            Console.WriteLine("Input matrix should be at least 2x2.");
            return;
        }

        var isInputSymetric = SymetricDetector.IsSymetric(input);
        if (isInputSymetric)
        {
            Console.WriteLine("Input matrix is symetric.");
        }
        else
        {
            Console.WriteLine("Input matrix is not symetric.");
            return;
        }

        var transversal = TransversalDetector.FindTransversal(input);
        if (transversal > 0)
        {
            Console.WriteLine($"Input matrix has transversal: {transversal}.");
        }
        else
        {
            Console.WriteLine($"Input matrix has no transversal.");
            return;
        }

        Console.WriteLine();

        var decompositor = new Decompositor(input, transversal, WriteOutput);
        decompositor.Decompose();

        Console.WriteLine();
        stopwatch.Stop();
        Console.WriteLine($"Elapsed: {stopwatch.ElapsedMilliseconds * 0.001:0.00}s");

        using var proc = Process.GetCurrentProcess();
        var memory = proc.PrivateMemorySize64 / (1024 * 1024);
        Console.WriteLine($"Memory used: {memory} Mb");

        Console.WriteLine("Done. Press <Enter> to exit...");
        Console.ReadLine();
    }

    private static void WriteOutput(IEnumerable<bool[,]> res)
    {
        foreach (var matrix in res)
        {
            ConsoleWriter.Write(matrix);
            var isSymetric = SymetricDetector.IsSymetric(matrix);
            Console.WriteLine(isSymetric ? "Matrix is symetric." : "Matrix not symetric.");
        }
        Console.WriteLine();
    }
}
