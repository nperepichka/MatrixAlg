using MatrixAlg.Analysers;

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
                Console.Write(matrix[i, j] ? 1 : 0);
            }
            // Write empty line to console
            Console.WriteLine();
        }
    }

    /// <summary>
    /// Write decomposition output to console
    /// </summary>
    /// <param name="res">List of decompositions on 1-transversals</param>
    /// <param name="n">Number of decomposition</param>
    public static void WriteDecomposition(byte[][] res, ulong n)
    {
        // Output decomposition counter value to console
        Console.WriteLine($"Decomposition #{n}");

        // Define a list to store all not symetric matrixes with indexes
        var notSymetricMatrixes = new List<(bool[,] matrix, int index)>();

        // Build and enumerate all matrixes
        foreach (var matrix in res.Select(BuildMatrix))
        {
            // Check if matrix is symetric
            var isSymetric = SymetricDetector.IsSymetric(matrix);
            // If is symetric
            if (isSymetric)
            {
                // Output matrix to console
                ConsoleWriter.WriteMatrix(matrix);
                // Write to console that matrix is symetric
                Console.WriteLine("Matrix is symetric.");
            }
            // If is not symetric
            else
            {
                // Add matrix to list of all not symetric matrixes
                notSymetricMatrixes.Add((matrix, notSymetricMatrixes.Count));
            }
        }

        // Enumerate all not symetric matrixes
        foreach (var (matrix, index) in notSymetricMatrixes)
        {
            // Output matrix to console
            ConsoleWriter.WriteMatrix(matrix);
            // Write to console that matrix is not symetric
            Console.WriteLine("Matrix is not symetric.");

            // Check if similar not symetric matrix exists
            var similarExists = notSymetricMatrixes.Any(m => m.index != index && SymetricDetector.AreSimilar(matrix, m.matrix));
            // If similar not symetric matrix exists
            if (similarExists)
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

        // Write empty line to console
        Console.WriteLine();
    }

    private static bool[,] BuildMatrix(byte[] indexes)
    {
        var size = indexes.Length;
        var res = new bool[size, size];
        for (var i = 0; i < size; i++)
        {
            for (var j = 0; j < size; j++)
            {
                res[i, j] = indexes[i] == j;
            }
        }
        return res;
    }
}
