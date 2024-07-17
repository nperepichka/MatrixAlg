namespace MatrixAlg.Helpers
{
    internal static class InputReader
    {
        public static bool[,] Read()
        {
            var lines = File
                .ReadAllLines("input.txt")
                .Select(_ => _.Trim())
                .Where(_ => !string.IsNullOrEmpty(_))
                .ToArray();

            var size = lines.Length;
            if (lines.Any(_ => _.Length != size))
            {
                throw new Exception($"Input matrix is not square.");
            }
            if (size < 2)
            {
                throw new Exception($"Input matrix should be at least 2x2.");
            }

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
