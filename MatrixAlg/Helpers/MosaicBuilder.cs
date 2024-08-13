using MatrixAlg.Analysers;

namespace MatrixAlg.Helpers;

/// <summary>
/// Helper class, used to build mosaics
/// </summary>
internal static class MosaicBuilder
{
    public static bool[,] BuildMosaic(bool[][,] matrixes)
    {
        var size = matrixes[0].GetLength(0);
        var halfSize = size / 2;

        var mosaic = new bool[size, size];
        var list = new List<bool[,]>();

        var m1Count = 0;
        var m2Count = 0;

        foreach (var matrix in matrixes)
        {
            var shouldUse = m1Count < halfSize && !list.Any(m => MatrixСonjugationDetector.AreСonjugate(matrix, m)) || m2Count >= halfSize;

            if (shouldUse)
            {
                list.Add(matrix);
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
                    if (matrix[i, j])
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
}
