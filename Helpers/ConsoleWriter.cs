using MatrixAlg.Analysers;
using MatrixAlg.Models;

namespace MatrixAlg.Helpers;

/// <summary>
/// Helper class, used to output matrix into console
/// </summary>
internal static class ConsoleWriter
{
    /// <summary>
    /// Output matrix into console
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
                // Write cell value to console
                Console.Write(matrix[i, j] ? " *" : " O");
            }
            // Write empty line to console
            Console.WriteLine();
        }
    }

    /// <summary>
    /// Write decomposition output to console
    /// </summary>
    /// <param name="decomposition">Decompositions on 1-transversals</param>
    /// <param name="n">Number of decomposition</param>
    public static void WriteDecomposition(byte[][] decomposition, ulong n)
    {
        // Output decomposition counter value to console
        Console.WriteLine($"Decomposition #{n}");

        // Define a list with matrixes details
        var matrixes = decomposition
            .Select((matrixElements, index) => new DecompositionMatrixDetails(matrixElements, index))
            .ToArray();

        // Check if cube of decomposition is symetric
        var isCubeSymetric = CubeSymetricDetector.IsSymetric(matrixes, out var sortedMartixes);
        // If cube is symetric
        if (isCubeSymetric)
        {
            // Write to console that cube of decomposition is symetric
            Console.WriteLine("Cube of decomposition is symetric.");
        }
        // If cube is not symetric
        else
        {
            // Write to console that cube of decomposition is not symetric
            Console.WriteLine("Cube of decomposition is not symetric.");
        }

        // Enumerate all matrixes
        foreach (var matrix in sortedMartixes)
        {
            // Output matrix to console
            WriteMatrix(matrix.Matrix);

            // If is symetric
            if (matrix.IsSymetric)
            {
                // Write to console that matrix is symetric
                Console.WriteLine("Matrix is symetric.");
            }
            // If is not symetric
            else
            {
                // Write to console that matrix is not symetric
                Console.WriteLine("Matrix is not symetric.");

                // Check if has similar not symetric matrix exists
                matrix.HasSimilar = sortedMartixes.Any(m => m.Index != matrix.Index && !m.IsSymetric && MatrixSimilarDetector.AreSimilar(matrix.Matrix, m.Matrix));
                // If similar not symetric matrix exists
                if (matrix.HasSimilar)
                {
                    // Write to console that matrix is similar to other not symetric matrix
                    Console.WriteLine("Matrix is similar to other not symetric matrix.");
                }
                // If similar not symetric matrix not exists
                else
                {
                    // Write to console that matrix is not similar to other not symetric matrix
                    Console.WriteLine("Matrix is not similar to other not symetric matrix.");
                }
            }
        }

        // Write empty line to console
        Console.WriteLine();
    }
}
