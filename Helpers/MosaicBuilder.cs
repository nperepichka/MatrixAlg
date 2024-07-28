using MatrixAlg.Models;

namespace MatrixAlg.Helpers;

/// <summary>
/// Helper class, used to build mosaics
/// </summary>
internal static class MosaicBuilder
{
    public static List<bool[,]> BuildMosaics(DecompositionMatrixDetails[] decomposition)
    {
        var size = decomposition[0].MatrixElements.Length;
        var n = decomposition.Length / 2;

        var mosaic1 = BuildMatrix(size);
        var mosaic2 = BuildMatrix(size);

        foreach (var matrix in decomposition)
        {
            for (var i = 0; i < size; i++)
            {
                for (var j = 0; j < size; j++)
                {
                    if (matrix.Matrix[i, j])
                    {
                        if (matrix.Index < n)
                        {
                            mosaic1[i, j] = true;
                        }
                        else
                        {
                            mosaic2[i, j] = true;
                        }
                    }
                }
            }
        }

        return [mosaic1, mosaic2];
    }

    private static bool[,] BuildMatrix(int size)
    {
        var res = new bool[size, size];
        for (var i = 0; i < size; i++)
        {
            for (var j = 0; j < size; j++)
            {
                res[i, j] = false;
            }
        }
        return res;
    }
}
