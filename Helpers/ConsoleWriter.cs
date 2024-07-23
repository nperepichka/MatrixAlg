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
    public static void Write(bool[,] matrix)
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
}
