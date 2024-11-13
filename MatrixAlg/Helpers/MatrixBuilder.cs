namespace MatrixAlg.Helpers;

public static class MatrixBuilder
{
    public static bool[,] RotateMatrix(this bool[,] matrix, byte size)
    {
        var res = new bool[size, size];

        for (var i = 0; i < size; i++)
        {
            var newI = size - 1 - i;
            for (var j = 0; j < size; ++j)
            {
                if (matrix[i, j])
                {
                    res[j, newI] = true;
                    break;
                }
            }
        }

        return res;
    }
}
