namespace MatrixAlg.Analysers;

internal static class MatrixSymetricDetector
{
    public static bool IsSymetric(bool[,] matrix)
    {
        return IsMainDiagonalSymmetric(matrix)
            || IsSecondaryDiagonalSymmetric(matrix)
            || IsVerticalSymmetric(matrix)
            || IsHorizontalSymmetric(matrix);
    }

    private static bool IsMainDiagonalSymmetric(bool[,] matrix)
    {
        var size = matrix.GetLength(0);
        for (var i = 0; i < size; i++)
        {
            for (var j = 0; j < i; j++)
            {
                if (matrix[i, j] != matrix[j, i])
                {
                    return false;
                }
            }
        }
        return true;
    }

    private static bool IsSecondaryDiagonalSymmetric(bool[,] matrix)
    {
        var size = matrix.GetLength(0);
        for (var i = 0; i < size; i++)
        {
            for (var j = 0; j < size - i - 1; j++)
            {
                if (matrix[i, j] != matrix[size - j - 1, size - i - 1])
                {
                    return false;
                }
            }
        }
        return true;
    }

    private static bool IsVerticalSymmetric(bool[,] matrix)
    {
        var size = matrix.GetLength(0);
        for (var i = 0; i < size; i++)
        {
            for (var j = 0; j < size / 2; j++)
            {
                if (matrix[i, j] != matrix[i, size - j - 1])
                {
                    return false;
                }
            }
        }
        return true;
    }

    private static bool IsHorizontalSymmetric(bool[,] matrix)
    {
        var size = matrix.GetLength(0);
        for (var i = 0; i < size / 2; i++)
        {
            for (var j = 0; j < size; j++)
            {
                if (matrix[i, j] != matrix[size - i - 1, j])
                {
                    return false;
                }
            }
        }
        return true;
    }
}
