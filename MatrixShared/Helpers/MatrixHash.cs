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
}
