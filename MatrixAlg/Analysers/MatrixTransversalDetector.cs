namespace MatrixAlg.Analysers;

internal static class MatrixTransversalDetector
{
    public static byte FindTransversal(this bool[,] matrix)
    {
        var size = matrix.GetLength(0);
        var columnVal = new byte[size];
        var rowVal = new byte[size];

        for (byte i = 0; i < size; i++)
        {
            for (byte j = 0; j < size; j++)
            {
                if (matrix[i, j])
                {
                    columnVal[j]++;
                    rowVal[i]++;
                }
            }
        }

        var transversal = columnVal[0];
        for (byte i = 0; i < size; i++)
        {
            if (columnVal[i] != transversal || rowVal[i] != transversal)
            {
                return byte.MinValue;
            }
        }

        return transversal;
    }
}
