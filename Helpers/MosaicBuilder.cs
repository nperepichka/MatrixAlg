using MatrixAlg.Analysers;
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
        var halfSize = size / 2;

        var mosaic1 = BuildMatrix(size);
        var mosaic2 = BuildMatrix(size);

        var m1list = new List<bool[,]>();
        var m1Count = 0;
        var m2Count = 0;

        foreach (var matrix in decomposition)
        {
            var shouldUseInM1 = m1Count < halfSize && !m1list.Any(m => MatrixСonjugationDetector.AreСonjugate(matrix.Matrix, m)) || m2Count >= halfSize;

            if (shouldUseInM1)
            {
                m1list.Add(matrix.Matrix);
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
                        if (shouldUseInM1)
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
