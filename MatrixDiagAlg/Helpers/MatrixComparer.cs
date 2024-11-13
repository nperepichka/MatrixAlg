namespace MatrixDiagAlg.Helpers;

public static class MatrixComparer
{
    public static bool[,] GetMinMatrix(this bool[,] matrix, byte size, bool[,] minMatrix)
    {
        for (byte row = 0; row < size; row++)
        {
            for (byte col = 0; col < size; col++)
            {
                if (matrix[row, col])
                {
                    if (!minMatrix[row, col])
                    {
                        return minMatrix;
                    }
                }
                else if (minMatrix[row, col])
                {
                    return matrix;
                }
            }
        }
        return matrix;
    }
}
