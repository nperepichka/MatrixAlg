using MatrixAlg.Enums;
using MatrixAlg.Helpers;
using MatrixAlg.Models;
using System.Text;

namespace MatrixAlg.Analysers;

internal static class CubeInvariantDetector
{
    // TODO: rewrite this class don't to generate Combinations

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

    public static int GetInvariantCubesCount(DecompositionMatrixDetails[] decomposition, StringBuilder outputStringBuilder)
    {
        var res = 0;

        if (ApplicationConfiguration.OutputCubeDetails)
        {
            outputStringBuilder.AppendLine("\nCube variants");
        }

        foreach (var combination in Combinations)
        {
            var testDecomposition = combination
                .Select(i => decomposition[i])
                .ToArray();

            var topViews = ViewVariantsToString(testDecomposition, CubeView.Top);
            var rightView = ViewVariantsToString(testDecomposition, CubeView.Right)[0];
            var backView = ViewVariantsToString(testDecomposition, CubeView.Back)[0];
            var leftView = ViewVariantsToString(testDecomposition, CubeView.Left)[0];
            var frontView = ViewVariantsToString(testDecomposition, CubeView.Front)[0];
            var bottomView = ViewVariantsToString(testDecomposition, CubeView.Bottom)[0];

            if (ApplicationConfiguration.OutputCubeDetails)
            {
                foreach (var d in testDecomposition)
                {
                    outputStringBuilder.AppendLine();
                    DataOutputWriter.WriteMatrix(d.Matrix, outputStringBuilder);
                }
                outputStringBuilder.AppendLine();

                for (var i = 0; i < topViews.Count; i++)
                {
                    outputStringBuilder.AppendLine("top     " + topViews[i]);
                }
                outputStringBuilder.AppendLine("bottom  " + bottomView);
                outputStringBuilder.AppendLine("right   " + rightView);
                outputStringBuilder.AppendLine("left    " + leftView);
                outputStringBuilder.AppendLine("front   " + frontView);
                outputStringBuilder.AppendLine("back    " + backView);
            }

            if (
                   topViews.Any(tv => tv == rightView)
                && topViews.Any(tv => tv == backView)
                && topViews.Any(tv => tv == leftView)
                && topViews.Any(tv => tv == bottomView)
                && topViews.Any(tv => tv == frontView)
                )
            {
                res++;
                if (ApplicationConfiguration.OutputCubeDetails)
                {
                    outputStringBuilder.AppendLine("Invariant");
                }
            }
            else if (ApplicationConfiguration.OutputCubeDetails)
            {
                outputStringBuilder.AppendLine("Not invariant");
            }
        }

        return res;
    }

    private static List<string> ViewVariantsToString(DecompositionMatrixDetails[] decomposition, CubeView view)
    {
        var res = new List<string>();

        var matrixes = decomposition.Select(m => m.Matrix).ToArray();
        res.Add(ViewToString(matrixes, view));

        if (view == CubeView.Top)
        {
            for (int i = 0; i < matrixes.Length; i++)
            {
                matrixes[i] = RotateMatrix(matrixes[i]);
            }
            res.Add(ViewToString(matrixes, view));

            for (int i = 0; i < matrixes.Length; i++)
            {
                matrixes[i] = RotateMatrix(matrixes[i]);
            }
            res.Add(ViewToString(matrixes, view));

            for (int i = 0; i < matrixes.Length; i++)
            {
                matrixes[i] = RotateMatrix(matrixes[i]);
            }
            res.Add(ViewToString(matrixes, view));
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

    private static string ViewToString(bool[][,] matrixes, CubeView view)
    {
        var stringBuilder = new StringBuilder(string.Empty);
        var size = matrixes.Length;

        switch (view)
        {
            case CubeView.Top:
                for (var k = 0; k < size; k++)
                {
                    for (var i = 0; i < size; i++)
                    {
                        for (var j = 0; j < size; j++)
                        {

                            stringBuilder.Append(matrixes[k][i, j] ? "*" : "O");
                        }
                        stringBuilder.Append(' ');
                    }
                    stringBuilder.Append(' ');
                }
                break;
            case CubeView.Bottom:
                for (var k = size - 1; k >= 0; k--)
                {
                    for (var i = size - 1; i >= 0; i--)
                    {
                        for (var j = 0; j < size; j++)
                        {
                            stringBuilder.Append(matrixes[k][i, j] ? "*" : "O");
                        }
                        stringBuilder.Append(' ');
                    }
                    stringBuilder.Append(' ');
                }
                break;
            case CubeView.Left:
                for (var j = 0; j < size; j++)
                {
                    for (var k = 0; k < size; k++)
                    {
                        for (var i = 0; i < size; i++)
                        {
                            stringBuilder.Append(matrixes[k][i, j] ? "*" : "O");
                        }
                        stringBuilder.Append(' ');
                    }
                    stringBuilder.Append(' ');
                }
                break;
            case CubeView.Right:
                for (var j = size - 1; j >= 0; j--)
                {
                    for (var k = 0; k < size; k++)
                    {
                        for (var i = size - 1; i >= 0; i--)
                        {
                            stringBuilder.Append(matrixes[k][i, j] ? "*" : "O");
                        }
                        stringBuilder.Append(' ');
                    }
                    stringBuilder.Append(' ');
                }
                break;
            case CubeView.Front:
                for (var i = size - 1; i >= 0; i--)
                {
                    for (var k = 0; k < size; k++)
                    {
                        for (var j = 0; j < size; j++)
                        {
                            stringBuilder.Append(matrixes[k][i, j] ? "*" : "O");
                        }
                        stringBuilder.Append(' ');
                    }
                    stringBuilder.Append(' ');
                }
                break;
            case CubeView.Back:
                for (var i = 0; i < size; i++)
                {
                    for (var k = 0; k < size; k++)
                    {
                        for (var j = size - 1; j >= 0; j--)
                        {
                            stringBuilder.Append(matrixes[k][i, j] ? "*" : "O");
                        }
                        stringBuilder.Append(' ');
                    }
                    stringBuilder.Append(' ');
                }
                break;
        }

        return stringBuilder.ToString();
    }
}
