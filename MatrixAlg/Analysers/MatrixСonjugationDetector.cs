using MatrixAlg.Helpers;

namespace MatrixAlg.Analysers;

internal static class MatrixСonjugationDetector
{
    public static bool AreСonjugate(bool[,] matrix1, bool[,] matrix2)
    {
        if (AreSame(matrix1, matrix2))
        {
            return true;
        }

        var rotatedMatrix1 = MatrixBuilder.RotateMatrix(matrix1);
        if (AreSame(matrix2, rotatedMatrix1))
        {
            return true;
        }

        rotatedMatrix1 = MatrixBuilder.RotateMatrix(rotatedMatrix1);
        if (AreSame(matrix2, rotatedMatrix1))
        {
            return true;
        }

        rotatedMatrix1 = MatrixBuilder.RotateMatrix(rotatedMatrix1);
        if (AreSame(matrix2, rotatedMatrix1))
        {
            return true;
        }

        var transformedMatrix1 = MirrorMatrixD1(matrix1);
        if (AreSame(transformedMatrix1, matrix2))
        {
            return true;
        }

        transformedMatrix1 = MirrorMatrixD2(matrix1);
        if (AreSame(transformedMatrix1, matrix2))
        {
            return true;
        }

        transformedMatrix1 = MirrorMatrixH(matrix1);
        if (AreSame(transformedMatrix1, matrix2))
        {
            return true;
        }

        transformedMatrix1 = MirrorMatrixV(matrix1);
        if (AreSame(transformedMatrix1, matrix2))
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

    public static bool IsSelfConjugate(bool[,] matrix)
    {
        var rotatedMatrix = MatrixBuilder.RotateMatrix(matrix);
        if (AreSame(matrix, rotatedMatrix))
        {
            return true;
        }

        rotatedMatrix = MatrixBuilder.RotateMatrix(rotatedMatrix);
        if (AreSame(matrix, rotatedMatrix))
        {
            return true;
        }

        rotatedMatrix = MatrixBuilder.RotateMatrix(rotatedMatrix);
        if (AreSame(matrix, rotatedMatrix))
        {
            return true;
        }

        return false;
    }
}
