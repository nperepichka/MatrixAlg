namespace MatrixAlg.Helpers
{
    internal static class InputReader
    {
        public static bool[,] Read()
        {
            var lines = File
                .ReadAllLines("input.txt")
                .Select(_ => _.Trim())
                .ToArray();

            var size = lines.Length;
            var res = new bool[size, size];

            for (var i = 0; i < size; i++)
            {
                for (var j = 0; j < size; j++)
                {
                    res[i, j] = lines[i][j] != '0';
                }
            }

            return res;
        }
    }
}
