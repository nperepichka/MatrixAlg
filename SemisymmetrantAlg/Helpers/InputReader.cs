namespace SemisymmetrantAlg.Helpers;

/// <summary>
/// Helper class, used to read input matrix from input.txt file
/// </summary>
public static class InputReader
{
    /// <summary>
    /// Read input matrix from file
    /// </summary>
    /// <returns>Input matrix</returns>
    public static int[,] ReadMatrix()
    {
        var lines = File
            .ReadAllLines("input.txt")
            .Select(l => l.Trim())
            .Where(l => !string.IsNullOrEmpty(l))
            .Select(l => l.Split(' ', StringSplitOptions.RemoveEmptyEntries))
            .ToArray();

        var size = lines.Length;
        if (lines.Any(_ => _.Length != size))
        {
            throw new Exception($"Input is not a matrix.");
        }
        if (size < 2)
        {
            throw new Exception($"Input matrix should be at least 2x2x2.");
        }
        if (size > 250)
        {
            throw new Exception($"Input matrix should be less or equal to 250x250.");
        }

        var res = new int[size, size];

        for (var i = 0; i < size; i++)
        {
            for (var j = 0; j < size; j++)
            {
                res[i, j] = int.Parse(lines[i][j]);
            }
        }

        return res;
    }
}
