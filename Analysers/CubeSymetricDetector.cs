using MatrixAlg.Enums;
using MatrixAlg.Models;
using System.Text;

namespace MatrixAlg.Analysers;

internal static class CubeSymetricDetector
{
    private static readonly CubeView[] CubeViews = Enum
        .GetValues(typeof(CubeView))
        .OfType<CubeView>().ToArray();

    private static readonly List<int[]> Combinations = [];

    public static void GenerateCombinations(int size)
    {
        Combinations.Clear();
        var indexes = new int[size];
        for (var i = 0; i < size; i++)
        {
            indexes[i] = i;
        }
        Permute(indexes, 0);
    }

    private static void Permute(int[] indexes, int start)
    {
        if (start >= indexes.Length - 1)
        {
            Combinations.Add([.. indexes]);
            return;
        }

        for (int i = start; i < indexes.Length; i++)
        {
            // Swap elements in place
            (indexes[start], indexes[i]) = (indexes[i], indexes[start]);

            Permute(indexes, start + 1);

            // Backtrack by swapping back
            (indexes[start], indexes[i]) = (indexes[i], indexes[start]);
        }
    }

    public static bool IsSymetric(DecompositionMatrixDetails[] decomposition, out DecompositionMatrixDetails[] sortedDecomposition)
    {
        foreach (var combination in Combinations)
        {
            var testDecomposition = combination
                .Select(index => decomposition.First(m => m.Index == index))
                .ToArray();

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

    private static string ViewToString(DecompositionMatrixDetails[] decomposition, CubeView view)
    {
        var stringBuilder = new StringBuilder(string.Empty);
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
                            stringBuilder.Append(decomposition[k].Matrix[i, j] ? "1" : "0");
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
                            // stringBuilder.Append(decomposition[k].Matrix[i, j] ? "1" : "0");
                            stringBuilder.Append(decomposition[size - k - 1].Matrix[i, j] ? "1" : "0");
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
                            stringBuilder.Append(decomposition[k].Matrix[i, j] ? "1" : "0");
                            //stringBuilder.Append(decomposition[k].Matrix[i, size - j - 1] ? "1" : "0"); // -
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
                            //stringBuilder.Append(decomposition[k].Matrix[i, j] ? "1" : "0");
                            stringBuilder.Append(decomposition[k].Matrix[i, size - j - 1] ? "1" : "0");
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
                            stringBuilder.Append(decomposition[k].Matrix[i, j] ? "1" : "0");
                            //stringBuilder.Append(decomposition[k].Matrix[size - i - 1, j] ? "1" : "0"); // -
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
                            //stringBuilder.Append(decomposition[k].Matrix[i, j] ? "1" : "0");
                            stringBuilder.Append(decomposition[k].Matrix[size - i - 1, j] ? "1" : "0");
                        }
                    }
                }
                break;
        }

        return stringBuilder.ToString();
    }
}
