namespace MatrixAlg.Helpers;

/// <summary>
/// Helper class, used to read input matrix from input.txt file
/// </summary>
internal static class InputReader
{
    /// <summary>
    /// Read input matrix from file
    /// </summary>
    /// <returns>Input matrix</returns>
    public static bool[,] Read()
    {
        // Read data form input.txt file
        var lines = File
            .ReadAllLines("input.txt")
            .Select(l => l.Trim())
            .Where(l => !string.IsNullOrEmpty(l))
            .ToArray();

        // Get input matrix size
        var size = lines.Length;
        // Check if input matrix is square
        if (lines.Any(_ => _.Length != size))
        {
            // Notify that input matrix is not square and interupt aplication execution
            throw new Exception($"Input matrix is not square.");
        }
        // Check if input matrix size is less than 2
        if (size < 2)
        {
            // Notify that input matrix is too small and interupt application execution
            throw new Exception($"Input matrix should be at least 2x2.");
        }
        // Check if input matrix size is more than 250
        if (size > 250)
        {
            // Notify that input matrix is too big and interupt application execution
            throw new Exception($"Input matrix should be less or equal to 250x250.");
        }

        // Define empty matrix
        var res = new bool[size, size];

        // Enumerate rows
        for (var i = 0; i < size; i++)
        {
            // Enumerate columns
            for (var j = 0; j < size; j++)
            {
                // Set cell value
                res[i, j] = lines[i][j] != '0';
            }
        }

        // Return input matrix
        return res;
    }
}
