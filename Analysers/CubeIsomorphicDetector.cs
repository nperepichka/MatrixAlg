using MatrixAlg.Enums;
using MatrixAlg.Helpers;
using MatrixAlg.Models;
using System.Text;

namespace MatrixAlg.Analysers;

internal static class CubeIsomorphicDetector
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

    public static int GetIsomorphicVariantsCount(DecompositionMatrixDetails[] decomposition, StringBuilder outputStringBuilder)
    {
        var res = 0;
        //outputStringBuilder.AppendLine("\nCube variants\n");

        foreach (var combination in Combinations)
        {
            var testDecomposition = combination
                .Select(i => decomposition[i])
                .ToArray();

            /*foreach (var d in testDecomposition)
            {
                DataOutputWriter.WriteMatrix(d.Matrix, outputStringBuilder);
                outputStringBuilder.AppendLine();
            }*/

            var topView = ViewVariantsToString(testDecomposition, CubeView.Top);
            var rightView = ViewVariantsToString(testDecomposition, CubeView.Right);
            var backView = ViewVariantsToString(testDecomposition, CubeView.Back);
            var leftView = ViewVariantsToString(testDecomposition, CubeView.Left);
            var frontView = ViewVariantsToString(testDecomposition, CubeView.Front);
            var bottomView = ViewVariantsToString(testDecomposition, CubeView.Bottom);

            /*for (var i = 0; i < topView.Count; i++)
            {
                outputStringBuilder.AppendLine("top     " + topView[i]);
                outputStringBuilder.AppendLine("bottom  " + bottomView[i]);
                outputStringBuilder.AppendLine("right   " + rightView[i]);
                outputStringBuilder.AppendLine("left    " + leftView[i]);
                outputStringBuilder.AppendLine("front   " + frontView[i]);
                outputStringBuilder.AppendLine("back    " + backView[i]);
                outputStringBuilder.AppendLine();
            }*/

            if (
                   topView.Any(t => rightView.Any(r => r == t)) && topView.Any(t => backView.Any(b => b == t))
                || topView.Any(t => leftView.Any(l => l == t)) && topView.Any(t => backView.Any(b => b == t))
                || topView.Any(t => rightView.Any(r => r == t)) && topView.Any(t => frontView.Any(f => f == t))
                || topView.Any(t => leftView.Any(l => l == t)) && topView.Any(t => frontView.Any(f => f == t))
                || bottomView.Any(b => rightView.Any(r => r == b)) && bottomView.Any(t => backView.Any(b => b == t))
                || bottomView.Any(b => leftView.Any(l => l == b)) && bottomView.Any(t => backView.Any(b => b == t))
                || bottomView.Any(b => rightView.Any(r => r == b)) && bottomView.Any(t => frontView.Any(f => f == t))
                || bottomView.Any(b => leftView.Any(l => l == b)) && bottomView.Any(t => frontView.Any(f => f == t))
                )
            {
                res++;
                /*outputStringBuilder.AppendLine("Isomorphic\n");
            }
            else
            {
                outputStringBuilder.AppendLine("Not isomorphic\n");*/
            }
        }

        return res;
    }

    private static List<string> ViewVariantsToString(DecompositionMatrixDetails[] decomposition, CubeView view)
    {
        var res = new List<string>();

        var matrixes = decomposition.Select(m => m.Matrix).ToArray();
        res.Add(ViewToString(matrixes, view));

        for (int i = 0; i < matrixes.Length; i++)
        {
            matrixes[i] = RotateMatrix(matrixes[i]);
        }
        res.Add(ViewToString(matrixes, view));

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

                            stringBuilder.Append(matrixes[k][i, j] ? "1" : "0");
                        }
                        stringBuilder.Append(" ");
                    }
                    stringBuilder.Append(" ");
                }
                break;
            case CubeView.Bottom:
                for (var k = size - 1; k >= 0; k--)
                {
                    for (var i = size - 1; i >= 0; i--)
                    {
                        for (var j = 0; j < size; j++)
                        {
                            stringBuilder.Append(matrixes[k][i, j] ? "1" : "0");
                        }
                        stringBuilder.Append(" ");
                    }
                    stringBuilder.Append(" ");
                }
                break;
            case CubeView.Left:
                for (var j = 0; j < size; j++)
                {
                    for (var k = 0; k < size; k++)
                    {
                        for (var i = 0; i < size; i++)
                        {
                            stringBuilder.Append(matrixes[k][i, j] ? "1" : "0");
                        }
                        stringBuilder.Append(" ");
                    }
                    stringBuilder.Append(" ");
                }
                break;
            case CubeView.Right:
                for (var j = size - 1; j >= 0; j--)
                {
                    for (var k = 0; k < size; k++)
                    {
                        for (var i = size - 1; i >= 0; i--)
                        {
                            stringBuilder.Append(matrixes[k][i, j] ? "1" : "0");
                        }
                        stringBuilder.Append(" ");
                    }
                    stringBuilder.Append(" ");
                }
                break;
            case CubeView.Front:
                for (var i = size - 1; i >= 0; i--)
                {
                    for (var k = 0; k < size; k++)
                    {
                        for (var j = 0; j < size; j++)
                        {
                            stringBuilder.Append(matrixes[k][i, j] ? "1" : "0");
                        }
                        stringBuilder.Append(" ");
                    }
                    stringBuilder.Append(" ");
                }
                break;
            case CubeView.Back:
                for (var i = 0; i < size; i++)
                {
                    for (var k = 0; k < size; k++)
                    {
                        for (var j = size - 1; j >= 0; j--)
                        {
                            stringBuilder.Append(matrixes[k][i, j] ? "1" : "0");
                        }
                        stringBuilder.Append(" ");
                    }
                    stringBuilder.Append(" ");
                }
                break;
        }

        return stringBuilder.ToString();
    }
}
