namespace AbsolutelySymmetricCubeAlg.Helpers;

/// <summary>
/// Helper class, used to read input cube from input.txt file
/// </summary>
public static class InputReader
{
    /// <summary>
    /// Read input cube from file
    /// </summary>
    /// <returns>Input cube</returns>
    public static bool[,,] ReadCube()
    {
        var lines = File
            .ReadAllLines("input.txt")
            .Select(l => l.Trim())
            .Where(l => !string.IsNullOrEmpty(l))
            .ToArray();

        var size = lines.Length > 0 ? lines[0].Length : 0;
        if (lines.Any(_ => _.Length != size) || lines.Length != size * size)
        {
            throw new Exception($"Input is not a cube.");
        }
        if (size < 2)
        {
            throw new Exception($"Input cube should be at least 2x2x2.");
        }
        if (size > 250)
        {
            throw new Exception($"Input cube should be less or equal to 250x250x250.");
        }

        var res = new bool[size, size, size];

        for (var s = 0; s < size; s++)
        {
            for (var i = 0; i < size; i++)
            {
                for (var j = 0; j < size; j++)
                {
                    res[s, i, j] = lines[s * size + i][j] != '0';
                }
            }
        }

        return res;
    }
}
