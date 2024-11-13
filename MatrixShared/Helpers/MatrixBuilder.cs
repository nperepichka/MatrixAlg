namespace MatrixShared.Helpers;

public static class MatrixBuilder
{
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
            var newJ = size - 1 - i;
            for (var j = 0; j < size - i; j++)
            {
                var newI = size - 1 - j;
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
            var newI = size - 1 - i;
            for (var j = 0; j < size; j++)
            {
                res[i, j] = matrix[newI, j];
            }
        }

        return res;
    }
}
