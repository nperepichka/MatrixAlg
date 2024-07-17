namespace MatrixAlg.Analysers;

internal static class TransversalDetector
{
    public static int FindTransversal(bool[,] matrix)
    {
        var size = matrix.GetLength(0);
        var columnVal = Enumerable.Repeat(0, size).ToArray();
        var rowVal = Enumerable.Repeat(0, size).ToArray();

        for (var i = 0; i < size; i++)
        {
            for (var j = 0; j < size; j++)
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
            ? transversal : 0;
    }
}
