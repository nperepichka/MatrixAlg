using MatrixAlg.Enums;
using MatrixAlg.Models;
using System.Text;

namespace MatrixAlg.Analysers;

internal static class SymetricDetector
{
    private static readonly CubeView[] CubeViews = Enum
        .GetValues(typeof(CubeView))
        .OfType<CubeView>().ToArray();

    private static readonly Dictionary<int, List<int[]>> Combinations = [];

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

    private static List<int[]> GetCombinations(int size)
    {
        if (!Combinations.TryGetValue(size, out List<int[]>? combinations))
        {
            var indexes = new int[size];
            for (var i = 0; i < size; i++)
            {
                indexes[i] = i;
            }
            combinations = [];
            Permute(indexes, 0, combinations);
            Combinations.Add(size, combinations);
        }

        return combinations;
    }

    public static bool IsCubeSymetric(DecompositionMatrixDetails[] decomposition, out DecompositionMatrixDetails[] sortedDecomposition)
    {
        var combinations = GetCombinations(decomposition.Length);

        foreach (var combination in combinations)
        {
            var testDecomposition = combination.Select(index => decomposition.First(m => m.Index == index)).ToArray();

            var views = CubeViews
                .ToDictionary(view => view, view => CubeViewToString(testDecomposition, view));

            if (
                views[CubeView.Top] == views[CubeView.Right] && views[CubeView.Right] == views[CubeView.Back]
                || views[CubeView.Top] == views[CubeView.Left] && views[CubeView.Left] == views[CubeView.Front]
                )
            {
                sortedDecomposition = testDecomposition;
                return true;
            }
        }

        sortedDecomposition = decomposition;
        return false;
    }

    private static void Permute(int[] indexes, int start, List<int[]> result)
    {
        if (start >= indexes.Length - 1)
        {
            result.Add((int[])indexes.Clone());
            return;
        }

        for (int i = start; i < indexes.Length; i++)
        {
            // Swap elements in place
            (indexes[start], indexes[i]) = (indexes[i], indexes[start]);

            Permute(indexes, start + 1, result);

            // Backtrack by swapping back
            (indexes[start], indexes[i]) = (indexes[i], indexes[start]);
        }
    }

    private static string CubeViewToString(DecompositionMatrixDetails[] decomposition, CubeView view)
    {
        var sb = new StringBuilder(string.Empty);
        var size = decomposition.Length;

        switch (view)
        {
            case CubeView.Top:
                for (var i = 0; i < size; i++)
                {
                    for (var j = 0; j < size; j++)
                    {
                        for (var k = 0; k < size; k++)
                        {
                            sb.Append(decomposition[k].Matrix[i, j] ? "1" : "0");
                        }
                    }
                }
                break;
            case CubeView.Bottom:
                for (var i = size - 1; i >= 0; i--)
                {
                    for (var j = 0; j < size; j++)
                    {
                        for (var k = 0; k < size; k++)
                        {
                            // sb.Append(decomposition[k].Matrix[i, j] ? "1" : "0");
                            sb.Append(decomposition[size - k - 1].Matrix[i, j] ? "1" : "0");
                        }
                    }
                }
                break;
            case CubeView.Left:
                for (var j = 0; j < size; j++)
                {
                    for (var k = 0; k < size; k++)
                    {
                        for (var i = 0; i < size; i++)
                        {
                            sb.Append(decomposition[k].Matrix[i, j] ? "1" : "0");
                            //sb.Append(decomposition[k].Matrix[i, size - j - 1] ? "1" : "0"); // -
                        }
                    }
                }
                break;
            case CubeView.Right:
                for (var j = 0; j < size; j++)
                {
                    for (var k = size - 1; k >= 0; k--)
                    {
                        for (var i = 0; i < size; i++)
                        {
                            //sb.Append(decomposition[k].Matrix[i, j] ? "1" : "0");
                            sb.Append(decomposition[k].Matrix[i, size - j - 1] ? "1" : "0");
                        }
                    }
                }
                break;
            case CubeView.Front:
                for (var j = 0; j < size; j++)
                {
                    for (var i = 0; i < size; i++)
                    {
                        for (var k = 0; k < size; k++)
                        {
                            sb.Append(decomposition[k].Matrix[i, j] ? "1" : "0");
                            //sb.Append(decomposition[k].Matrix[size - i - 1, j] ? "1" : "0"); // -
                        }
                    }
                }
                break;
            case CubeView.Back:
                for (var j = size - 1; j >= 0; j--)
                {
                    for (var i = 0; i < size; i++)
                    {
                        for (var k = 0; k < size; k++)
                        {
                            //sb.Append(decomposition[k].Matrix[i, j] ? "1" : "0");
                            sb.Append(decomposition[k].Matrix[size - i - 1, j] ? "1" : "0");
                        }
                    }
                }
                break;
        }

        return sb.ToString();
    }

    public static bool AreSimilar(bool[,] matrix1, bool[,] matrix2)
    {
        if (AreSame(matrix1, matrix2))
        {
            return true;
        }

        var matrix1r1 = RotateMatrix(matrix1);
        if (AreSame(matrix1r1, matrix2))
        {
            return true;
        }

        var matrix1r2 = RotateMatrix(matrix1r1);
        if (AreSame(matrix1r2, matrix2))
        {
            return true;
        }

        var matrix1r3 = RotateMatrix(matrix1r2);
        if (AreSame(matrix1r3, matrix2))
        {
            return true;
        }

        var matrix1d1 = MirrorMatrixD1(matrix1);
        if (AreSame(matrix1d1, matrix2))
        {
            return true;
        }

        var matrix1d2 = MirrorMatrixD2(matrix1);
        if (AreSame(matrix1d2, matrix2))
        {
            return true;
        }

        var matrix1h = MirrorMatrixH(matrix1);
        if (AreSame(matrix1h, matrix2))
        {
            return true;
        }

        var matrix1v = MirrorMatrixV(matrix1);
        if (AreSame(matrix1v, matrix2))
        {
            return true;
        }

        return false;
    }

    private static bool[,] MirrorMatrixD1(bool[,] matrix)
    {
        var size = matrix.GetLength(0);
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

    private static bool[,] MirrorMatrixD2(bool[,] matrix)
    {
        var size = matrix.GetLength(0);
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
            for (var j = 0; j < size - i; j++)
            {
                var newI = size - 1 - j;
                var newJ = size - 1 - i;
                res[i, j] = matrix[newI, newJ];
                res[newI, newJ] = matrix[i, j];
            }
        }

        return res;
    }

    private static bool[,] MirrorMatrixV(bool[,] matrix)
    {
        var size = matrix.GetLength(0);
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

    private static bool[,] MirrorMatrixH(bool[,] matrix)
    {
        var size = matrix.GetLength(0);
        var res = new bool[size, size];

        for (var i = 0; i < size; i++)
        {
            for (var j = 0; j < size; j++)
            {
                res[i, j] = matrix[size - 1 - i, j];
            }
        }

        return res;
    }

    private static bool[,] RotateMatrix(bool[,] matrix)
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

    private static bool AreSame(bool[,] matrix1, bool[,] matrix2)
    {
        var size = matrix1.GetLength(0);

        for (var i = 0; i < size; i++)
        {
            for (var j = 0; j < size; j++)
            {
                if (matrix1[i, j] != matrix2[i, j])
                {
                    return false;
                }
            }
        }

        return true;
    }
}
