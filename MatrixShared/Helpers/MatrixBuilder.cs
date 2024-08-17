namespace MatrixShared.Helpers;

public static class MatrixBuilder
{
    public static bool[,] BuildMatrix(this byte[] elements)
    {
        var size = elements.Length;
        var res = new bool[size, size];

        for (var i = 0; i < size; i++)
        {
            res[i, elements[i]] = true;
        }

        return res;
    }

    public static bool[,] RotateMatrix(this bool[,] matrix)
    {
        var size = matrix.GetLength(0);
        var res = new bool[size, size];

        for (var i = 0; i < size; i++)
        {
            for (var j = 0; j < size; ++j)
            {
                if (matrix[i, j])
                {
                    res[j, size - i - 1] = true;
                    break;
                }
            }
        }

        return res;
    }
}
