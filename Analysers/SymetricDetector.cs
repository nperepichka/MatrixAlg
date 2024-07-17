namespace MatrixAlg.Analysers;

internal static class SymetricDetector
{
    public static bool IsSymetric(bool[,] matrix)
    {
        var size = matrix.GetLength(0);
        for (var i = 0; i < size; i++)
        {
            for (var j = 0; j < size; j++)
            {
                if (matrix[i, j] != matrix[j, i])
                {
                    return false;
                }
            }
        }

        return true;
    }

    public static bool AreSimilar(bool[,] matrix1, bool[,] matrix2)
    {
        if (AreSame(matrix1, matrix2))
        {
            return true;
        }

        var matrix1r1 = RotateMatrix(matrix1);
        if (AreSame(matrix1r1, matrix2))
        {
            return true;
        }

        var matrix1r2 = RotateMatrix(matrix1r1);
        if (AreSame(matrix1r2, matrix2))
        {
            return true;
        }

        var matrix1r3 = RotateMatrix(matrix1r2);
        if (AreSame(matrix1r3, matrix2))
        {
            return true;
        }

        var matrix1d1 = MirrorMatrixD1(matrix1);
        if (AreSame(matrix1d1, matrix2))
        {
            return true;
        }

        var matrix1d2 = MirrorMatrixD2(matrix1);
        if (AreSame(matrix1d2, matrix2))
        {
            return true;
        }

        var matrix1h = MirrorMatrixH(matrix1);
        if (AreSame(matrix1h, matrix2))
        {
            return true;
        }

        var matrix1v = MirrorMatrixV(matrix1);
        if (AreSame(matrix1v, matrix2))
        {
            return true;
        }

        return false;
    }

    private static bool[,] MirrorMatrixD1(bool[,] matrix)
    {
        var size = matrix.GetLength(0);
        var res = new bool[size, size];

        for (var i = 0; i < size; i++)
        {
            for (var j = 0; j < size; j++)
            {
                res[i, j] = matrix[i, j];
            }
        }

        for (var i = 0; i < size; i++)
        {
            for (var j = i + 1; j < size; j++)
            {
                res[i, j] = matrix[j, i];
                res[j, i] = matrix[i, j];
            }
        }

        return res;
    }

    private static bool[,] MirrorMatrixD2(bool[,] matrix)
    {
        var size = matrix.GetLength(0);
        var res = new bool[size, size];

        for (var i = 0; i < size; i++)
        {
            for (var j = 0; j < size; j++)
            {
                res[i, j] = matrix[i, j];
            }
        }

        for (var i = 0; i < size; i++)
        {
            for (var j = 0; j < size - i; j++)
            {
                var newI = size - 1 - j;
                var newJ = size - 1 - i;
                res[i, j] = matrix[newI, newJ];
                res[newI, newJ] = matrix[i, j];
            }
        }

        return res;
    }

    private static bool[,] MirrorMatrixV(bool[,] matrix)
    {
        var size = matrix.GetLength(0);
        var res = new bool[size, size];

        for (var i = 0; i < size; i++)
        {
            for (var j = 0; j < size; j++)
            {
                res[i, j] = matrix[i, size - 1 - j];
            }
        }

        return res;
    }

    private static bool[,] MirrorMatrixH(bool[,] matrix)
    {
        var size = matrix.GetLength(0);
        var res = new bool[size, size];

        for (var i = 0; i < size; i++)
        {
            for (var j = 0; j < size; j++)
            {
                res[i, j] = matrix[size - 1 - i, j];
            }
        }

        return res;
    }

    private static bool[,] RotateMatrix(bool[,] matrix)
    {
        var size = matrix.GetLength(0);
        var res = new bool[size, size];

        for (var i = size - 1; i >= 0; --i)
        {
            for (var j = 0; j < size; ++j)
            {
                res[j, size - i - 1] = matrix[i, j];
            }
        }

        return res;
    }

    private static bool AreSame(bool[,] matrix1, bool[,] matrix2)
    {
        var size = matrix1.GetLength(0);

        for (var i = 0; i < size; i++)
        {
            for (var j = 0; j < size; j++)
            {
                if (matrix1[i, j] != matrix2[i, j])
                {
                    return false;
                }
            }
        }

        return true;
    }
}
