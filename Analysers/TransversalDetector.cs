namespace MatrixAlg.Analysers;

internal static class TransversalDetector
{
    public static byte FindTransversal(bool[,] matrix)
    {
        var size = matrix.GetLength(0);
        var columnVal = Enumerable.Repeat(byte.MinValue, size).ToArray();
        var rowVal = Enumerable.Repeat(byte.MinValue, size).ToArray();

        for (byte i = 0; i < size; i++)
        {
            for (byte j = 0; j < size; j++)
            {
                if (matrix[i, j])
                {
                    columnVal[j]++;
                    rowVal[j]++;
                }
            }
        }

        var transversal = columnVal[0];

        return columnVal.All(_ => _ == transversal) && rowVal.All(_ => _ == transversal)
            ? transversal : byte.MinValue;
    }
}
