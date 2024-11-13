using MatrixAlg.Helpers;
using MatrixShared.Helpers;

namespace MatrixAlg.Analysers;

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
