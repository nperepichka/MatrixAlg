using MatrixAlg.Analysers;
using MatrixAlg.Helpers;
using System.Diagnostics;

namespace MatrixAlg;

internal class Program
{
    private static void Main()
    {
        var stopwatch = Stopwatch.StartNew();

        var input = InputReader.Read();

        Console.WriteLine("Input matrix:");
        ConsoleWriter.Write(input);

        var isInputSymetric = SymetricDetector.IsSymetric(input);
        if (isInputSymetric)
        {
            Console.WriteLine("Input matrix is symetric.");
        }
        else
        {
            Console.WriteLine("Input matrix is not symetric.");
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

        using var decompositor = new Decompositor(input, transversal, WriteOutput);
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
        var notSymetricMatrixes = new List<bool[,]>();

        foreach (var matrix in res)
        {
            var isSymetric = SymetricDetector.IsSymetric(matrix);
            if (isSymetric)
            {
                ConsoleWriter.Write(matrix);
                Console.WriteLine("Matrix is symetric.");
            }
            else
            {
                notSymetricMatrixes.Add(matrix);
            }
        }

        var list = notSymetricMatrixes.Select((matrix, i) => (matrix, i));
        foreach (var (matrix, i) in list)
        {
            ConsoleWriter.Write(matrix);
            Console.WriteLine("Matrix is not symetric.");

            var similarExists = list.Any(_ => _.i != i && SymetricDetector.AreSimilar(matrix, _.matrix));
            if (similarExists)
            {
                Console.WriteLine("Matrix is similar to other not symetric matrix.");
            }
            else
            {
                Console.WriteLine("Matrix is not similar to other not symetric matrix.");
            }
        }

        Console.WriteLine();
    }
}
