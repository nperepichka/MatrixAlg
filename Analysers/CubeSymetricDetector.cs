using MatrixAlg.Enums;
using MatrixAlg.Models;
using System.Text;

namespace MatrixAlg.Analysers;

internal static class CubeSymetricDetector
{
    private static readonly CubeView[] CubeViews = Enum
        .GetValues(typeof(CubeView))
        .OfType<CubeView>().ToArray();

    private static readonly Dictionary<int, List<int[]>> Combinations = [];

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

    public static bool IsSymetric(DecompositionMatrixDetails[] decomposition, out DecompositionMatrixDetails[] sortedDecomposition)
    {
        var combinations = GetCombinations(decomposition.Length);

        foreach (var combination in combinations)
        {
            var testDecomposition = combination.Select(index => decomposition.First(m => m.Index == index)).ToArray();

            var views = CubeViews
                .ToDictionary(view => view, view => ViewToString(testDecomposition, view));

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

    private static string ViewToString(DecompositionMatrixDetails[] decomposition, CubeView view)
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
}
