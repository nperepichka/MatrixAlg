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

    /*public static IEnumerable<bool[,]> GenerateAllMatrices(int n, int m)
    {
        var rowPatterns = GenerateRowPatterns(n, m).ToList();
        int[] colSums = new int[n];
        var current = new bool[n, n];

        foreach (var matrix in Backtrack(n, m, 0, current, colSums, rowPatterns))
            yield return matrix;
    }

    private static IEnumerable<bool[,]> Backtrack(int n, int m, int row, bool[,] current, int[] colSums, List<bool[]> rowPatterns)
    {
        if (row == n)
        {
            if (colSums.All(c => c == m))
            {
                var result = new bool[n, n];
                Array.Copy(current, result, current.Length);
                yield return result;
            }
            yield break;
        }

        foreach (var pattern in rowPatterns)
        {
            bool valid = true;
            for (int j = 0; j < n; j++)
            {
                if (pattern[j] && colSums[j] + 1 > m)
                {
                    valid = false;
                    break;
                }
            }

            if (!valid) continue;

            for (int j = 0; j < n; j++)
            {
                current[row, j] = pattern[j];
                if (pattern[j]) colSums[j]++;
            }

            foreach (var matrix in Backtrack(n, m, row + 1, current, colSums, rowPatterns))
                yield return matrix;

            for (int j = 0; j < n; j++)
            {
                if (pattern[j]) colSums[j]--;
                current[row, j] = false;
            }
        }
    }

    private static IEnumerable<bool[]> GenerateRowPatterns(int n, int m)
    {
        foreach (var indices in GetCombinations(Enumerable.Range(0, n).ToList(), m))
        {
            var row = new bool[n];
            foreach (var idx in indices)
                row[idx] = true;
            yield return row;
        }
    }

    private static IEnumerable<List<int>> GetCombinations(List<int> list, int k, int start = 0)
    {
        if (k == 0)
        {
            yield return new List<int>();
            yield break;
        }

        for (int i = start; i <= list.Count - k; i++)
        {
            foreach (var tail in GetCombinations(list, k - 1, i + 1))
            {
                tail.Insert(0, list[i]);
                yield return tail;
            }
        }
    }*/
}
