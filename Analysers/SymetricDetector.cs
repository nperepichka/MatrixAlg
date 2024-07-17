namespace MatrixAlg.Analysers
{
    internal static class SymetricDetector
    {
        public static bool IsSymetric(bool[,] matrix)
        {
            var size = matrix.GetLength(0);
            for (var i = 0; i < size; i++)
            {
                for (var j = 0; j < size; j++)
                {
                    if (matrix[i, j] != matrix[j, i])
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
