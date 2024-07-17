namespace MatrixAlg.Helpers;

internal static class ConsoleWriter
{
    public static void Write(bool[,] matrix)
    {
        var size = matrix.GetLength(0);

        for (var i = 0; i < size; i++)
        {
            for (var j = 0; j < size; j++)
            {
                Console.Write(matrix[i, j] ? 1 : 0);
            }
            Console.WriteLine();
        }
    }
}
