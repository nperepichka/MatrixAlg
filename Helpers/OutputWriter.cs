using MatrixAlg.Analysers;
using MatrixAlg.Models;

namespace MatrixAlg.Helpers;

/// <summary>
/// Helper class, used to output matrix
/// </summary>
internal static class OutputWriter
{
    private const string OutputFileName = "output.txt";

    public static bool CanWriteToConsole = true;

    /// <summary>
    /// Write matrix
    /// </summary>
    /// <param name="matrix">Matrix</param>
    public static void WriteMatrix(bool[,] matrix)
    {
        // Get matrix size
        var size = matrix.GetLength(0);

        // Enumerate rows
        for (var i = 0; i < size; i++)
        {
            // Enumerate columns
            for (var j = 0; j < size; j++)
            {
                // Write cell value
                Write(matrix[i, j] ? " *" : " O");
            }
            // Write empty line
            WriteLine();
        }
    }

    /// <summary>
    /// Write decomposition output
    /// </summary>
    /// <param name="decomposition">Decompositions on 1-transversals</param>
    /// <param name="n">Number of decomposition</param>
    public static void WriteDecomposition(byte[][] decomposition, ulong n)
    {
        // Output decomposition counter value
        WriteLine($"Decomposition #{n}");

        // Define a list with matrixes details
        var matrixes = decomposition
            .Select((matrixElements, index) => new DecompositionMatrixDetails(matrixElements, index))
            .ToArray();

        // Check if cube of decomposition is symetric
        var isCubeSymetric = CubeSymetricDetector.IsSymetric(matrixes, out var sortedMartixes);
        // If cube is symetric
        if (isCubeSymetric)
        {
            // Write that cube of decomposition is symetric
            WriteLine("Cube of decomposition is symetric.");
        }
        // If cube is not symetric
        else
        {
            // Write that cube of decomposition is not symetric
            WriteLine("Cube of decomposition is not symetric.");
        }

        // Enumerate all matrixes
        foreach (var matrix in sortedMartixes)
        {
            // Output matrix
            WriteMatrix(matrix.Matrix);

            // If is symetric
            if (matrix.IsSymetric)
            {
                // Write that matrix is symetric
                WriteLine("Matrix is symetric.");
            }
            // If is self similar
            else if (matrix.IsSelfSimilar)
            {
                // Write that matrix is self similar
                WriteLine("Matrix is self similar.");
            }
            // If is not symetric and not self similar
            else
            {
                // Check if has similar not symetric matrix exists
                matrix.HasSimilar = sortedMartixes.Any(m => m.Index != matrix.Index && !m.IsSymetric && MatrixSimilarDetector.AreSimilar(matrix.Matrix, m.Matrix));
                // If similar not symetric matrix exists
                if (matrix.HasSimilar)
                {
                    // Write that matrix is similar to other matrix
                    WriteLine("Matrix is not symetric and not self similar, but is similar to other matrix.");
                }
                // If similar not symetric matrix not exists
                else
                {
                    // Write that matrix is not similar to other matrix
                    WriteLine("Matrix is not symetric, not self similar and not similar to other matrix.");
                }
            }
        }

        // Write empty line
        WriteLine();
    }

    public static void Clear()
    {
        if (File.Exists(OutputFileName))
        {
            File.Delete(OutputFileName);
        }
    }

    public static void Write(string s)
    {
        if (CanWriteToConsole)
        {
            Console.Write(s);
        }
        File.AppendAllText(OutputFileName, s);
    }

    public static void WriteLine(string? s = null)
    {
        if (CanWriteToConsole)
        {
            Console.WriteLine(s);
        }
        File.AppendAllText(OutputFileName, $"{s}{Environment.NewLine}");
    }
}
