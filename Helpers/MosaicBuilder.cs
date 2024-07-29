using MatrixAlg.Analysers;
using MatrixAlg.Models;

namespace MatrixAlg.Helpers;

/// <summary>
/// Helper class, used to build mosaics
/// </summary>
internal static class MosaicBuilder
{
    public static bool[,] BuildMosaic(DecompositionMatrixDetails[] decomposition)
    {
        var size = decomposition[0].Matrix.GetLength(0);
        var halfSize = size / 2;

        var mosaic = BuildMatrix(size);
        var list = new List<bool[,]>();

        var m1Count = 0;
        var m2Count = 0;

        foreach (var matrix in decomposition)
        {
            var shouldUse = m1Count < halfSize && !list.Any(m => MatrixСonjugationDetector.AreСonjugate(matrix.Matrix, m)) || m2Count >= halfSize;

            if (shouldUse)
            {
                list.Add(matrix.Matrix);
                m1Count++;
            }
            else
            {
                m2Count++;
            }

            for (var i = 0; i < size; i++)
            {
                for (var j = 0; j < size; j++)
                {
                    if (matrix.Matrix[i, j])
                    {
                        if (shouldUse)
                        {
                            mosaic[i, j] = true;
                        }
                    }
                }
            }
        }

        return mosaic;
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
