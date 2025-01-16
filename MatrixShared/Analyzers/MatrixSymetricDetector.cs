namespace MatrixShared.Analyzers;

public static class MatrixSymetricDetector
{
    public static bool IsSymetric(this bool[,] matrix)
    {
        return matrix.IsMainDiagonalSymmetric()
            || matrix.IsSecondaryDiagonalSymmetric()
            || matrix.IsVerticalSymmetric()
            || matrix.IsHorizontalSymmetric();
    }

    private static bool IsMainDiagonalSymmetric(this bool[,] matrix)
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

    private static bool IsSecondaryDiagonalSymmetric(this bool[,] matrix)
    {
        var size1 = matrix.GetLength(0) - 1;
        for (var i = 0; i <= size1; i++)
        {
            for (var j = 0; j < size1 - i; j++)
            {
                if (matrix[i, j] != matrix[size1 - j, size1 - i])
                {
                    return false;
                }
            }
        }
        return true;
    }

    private static bool IsVerticalSymmetric(this bool[,] matrix)
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

    private static bool IsHorizontalSymmetric(this bool[,] matrix)
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
