namespace MatrixAlg.Helpers
{
    internal static class MatrixBuilder
    {
        public static bool[,] BuildEmptyMatrix(int size)
        {
            var res = new bool[size, size];

            for (var i = 0; i < size; i++)
            {
                for (var j = 0; j < size; j++)
                {
                    res[i, j] = false;
                }
            }

            return res;
        }

        public static bool[,] BuildMatrix(byte[] elements)
        {
            var size = elements.Length;
            var res = new bool[size, size];

            for (var i = 0; i < size; i++)
            {
                for (var j = 0; j < size; j++)
                {
                    res[i, j] = elements[i] == j;
                }
            }

            return res;
        }

        public static bool[,] RotateMatrix(bool[,] matrix)
        {
            var size = matrix.GetLength(0);
            var res = new bool[size, size];

            for (var i = size - 1; i >= 0; --i)
            {
                for (var j = 0; j < size; ++j)
                {
                    res[j, size - i - 1] = matrix[i, j];
                }
            }

            return res;
        }
    }
}
