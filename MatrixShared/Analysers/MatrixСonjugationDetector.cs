using MatrixShared.Helpers;

namespace MatrixShared.Analysers;

public static class MatrixСonjugationDetector
{
    public static bool AreСonjugate(bool[,] matrix1, bool[,] matrix2)
    {
        var size = (byte)matrix1.GetLength(0);

        if (AreSame(matrix1, matrix2, size))
        {
            return true;
        }

        var rotatedMatrix1 = matrix1.RotateMatrix(size);
        if (AreSame(matrix2, rotatedMatrix1, size))
        {
            return true;
        }

        rotatedMatrix1 = rotatedMatrix1.RotateMatrix(size);
        if (AreSame(matrix2, rotatedMatrix1, size))
        {
            return true;
        }

        rotatedMatrix1 = rotatedMatrix1.RotateMatrix(size);
        if (AreSame(matrix2, rotatedMatrix1, size))
        {
            return true;
        }

        var transformedMatrix1 = matrix1.MirrorMatrixD1(size);
        if (AreSame(transformedMatrix1, matrix2, size))
        {
            return true;
        }

        transformedMatrix1 = matrix1.MirrorMatrixD2(size);
        if (AreSame(transformedMatrix1, matrix2, size))
        {
            return true;
        }

        transformedMatrix1 = matrix1.MirrorMatrixH(size);
        if (AreSame(transformedMatrix1, matrix2, size))
        {
            return true;
        }

        transformedMatrix1 = matrix1.MirrorMatrixV(size);
        if (AreSame(transformedMatrix1, matrix2, size))
        {
            return true;
        }

        return false;
    }

    public static bool[,] MirrorMatrixD1(this bool[,] matrix, byte size)
    {
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

    public static bool[,] MirrorMatrixD2(this bool[,] matrix, byte size)
    {
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

    public static bool[,] MirrorMatrixV(this bool[,] matrix, byte size)
    {
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

    public static bool[,] MirrorMatrixH(this bool[,] matrix, byte size)
    {
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

    private static bool AreSame(bool[,] matrix1, bool[,] matrix2, byte size)
    {
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

    public static bool IsSelfConjugate(this bool[,] matrix)
    {
        var size = (byte)matrix.GetLength(0);

        var rotatedMatrix = matrix.RotateMatrix(size);
        if (AreSame(matrix, rotatedMatrix, size))
        {
            return true;
        }

        rotatedMatrix = rotatedMatrix.RotateMatrix(size);
        if (AreSame(matrix, rotatedMatrix, size))
        {
            return true;
        }

        rotatedMatrix = rotatedMatrix.RotateMatrix(size);
        if (AreSame(matrix, rotatedMatrix, size))
        {
            return true;
        }

        return false;
    }
}
