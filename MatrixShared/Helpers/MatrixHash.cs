namespace MatrixShared.Helpers;

public static class MatrixHash
{
    public static string GetHash(this bool[,] matrix, byte size)
    {
        var flatArray = new char[size * size];

        var index = 0;
        for (byte row = 0; row < size; row++)
        {
            for (byte col = 0; col < size; col++)
            {
                flatArray[index++] = matrix[row, col] ? '1' : '0';
            }
        }

        return new string(flatArray);
    }

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
