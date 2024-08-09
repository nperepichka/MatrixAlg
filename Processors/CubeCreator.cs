using MatrixAlg.Enums;
using MatrixAlg.Helpers;
using MatrixAlg.Models;
using System.Text;

namespace MatrixAlg.Processors;

internal class CubeCreator(byte Size)
{
    private readonly object CubeLock = new();
    public readonly HashSet<string> CubeViews = [];

    // TODO: rewrite this class don't to generate Combinations
    private static readonly List<byte[]> Combinations = [];

    public static void GenerateCombinations(byte Size)
    {
        Combinations.Clear();
        var indexes = new byte[Size];
        for (byte i = 0; i < Size; i++)
        {
            indexes[i] = i;
        }
        Permute(indexes, 0);
    }

    private static void Permute(byte[] indexes, byte start)
    {
        if (start >= indexes.Length - 1)
        {
            Combinations.Add([.. indexes]);
            return;
        }

        var nextStart = start;
        nextStart++;

        for (var i = start; i < indexes.Length; i++)
        {
            // Swap elements in place
            (indexes[start], indexes[i]) = (indexes[i], indexes[start]);

            Permute(indexes, nextStart);

            // Backtrack by swapping back
            (indexes[start], indexes[i]) = (indexes[i], indexes[start]);
        }
    }

    public void CreateInvariantCubes(DecompositionMatrixDetails[] matrixes)
    {
        foreach (var combination in Combinations)
        {
            var matrixesCombination = combination
                .Select(i => matrixes[i])
                .ToArray();

            var topViews = ViewVariantsToString(matrixesCombination, CubeView.Top);

            if (
                   topViews.Any(tv => tv == ViewToString(matrixesCombination, CubeView.Right))
                && topViews.Any(tv => tv == ViewToString(matrixesCombination, CubeView.Back))
                && topViews.Any(tv => tv == ViewToString(matrixesCombination, CubeView.Left))
                && topViews.Any(tv => tv == ViewToString(matrixesCombination, CubeView.Front))
                // TODO: maybe we don't need next line
                && topViews.Any(tv => tv == ViewToString(matrixesCombination, CubeView.Bottom))
                )
            {
                var uniqueCandidateView = topViews.OrderByDescending(tv => tv).First();
                bool isUnique;
                lock (CubeLock)
                {
                    isUnique = CubeViews.Add(uniqueCandidateView);
                }
                if (isUnique)
                {
                    OutputWriter.WriteLine($"Unique invariant cube found:{Environment.NewLine}{FormatView(uniqueCandidateView)}{Environment.NewLine}");
                }
            }
        }
    }

    private string FormatView(string view)
    {
        var stringBuilder = new StringBuilder(string.Empty);
        var i1 = 0;
        var i2 = 0;

        foreach (var s in view)
        {
            i1++;
            stringBuilder.Append(s);
            if (i1 % Size == 0)
            {
                i2++;
                stringBuilder.Append(' ');
                if (i2 % Size == 0)
                {
                    stringBuilder.Append(' ');
                }
            }
        }

        return stringBuilder.ToString();
    }

    private List<string> ViewVariantsToString(DecompositionMatrixDetails[] decomposition, CubeView view)
    {
        var res = new List<string>(4);

        var matrixes = decomposition.Select(m => m.Matrix).ToArray();
        res.Add(ViewToString(matrixes, view));

        for (var i = 0; i < matrixes.Length; i++)
        {
            matrixes[i] = RotateMatrix(matrixes[i]);
        }
        res.Add(ViewToString(matrixes, view));

        for (var i = 0; i < matrixes.Length; i++)
        {
            matrixes[i] = RotateMatrix(matrixes[i]);
        }
        res.Add(ViewToString(matrixes, view));

        for (var i = 0; i < matrixes.Length; i++)
        {
            matrixes[i] = RotateMatrix(matrixes[i]);
        }
        res.Add(ViewToString(matrixes, view));

        return res;
    }

    private string ViewToString(DecompositionMatrixDetails[] decomposition, CubeView view)
    {
        var matrixes = decomposition.Select(m => m.Matrix).ToArray();
        return ViewToString(matrixes, view);
    }

    private bool[,] RotateMatrix(bool[,] matrix)
    {
        var res = new bool[Size, Size];

        for (var i = Size - 1; i >= 0; --i)
        {
            for (var j = 0; j < Size; ++j)
            {
                res[j, Size - i - 1] = matrix[i, j];
            }
        }

        return res;
    }

    private string ViewToString(bool[][,] matrixes, CubeView view)
    {
        var stringBuilder = new StringBuilder(string.Empty);

        switch (view)
        {
            case CubeView.Top:
                for (var k = 0; k < Size; k++)
                {
                    for (var i = 0; i < Size; i++)
                    {
                        for (var j = 0; j < Size; j++)
                        {

                            stringBuilder.Append(matrixes[k][i, j] ? "*" : "O");
                        }
                    }
                }
                break;
            case CubeView.Bottom:
                for (var k = Size - 1; k >= 0; k--)
                {
                    for (var i = Size - 1; i >= 0; i--)
                    {
                        for (var j = 0; j < Size; j++)
                        {
                            stringBuilder.Append(matrixes[k][i, j] ? "*" : "O");
                        }
                    }
                }
                break;
            case CubeView.Left:
                for (var j = 0; j < Size; j++)
                {
                    for (var k = 0; k < Size; k++)
                    {
                        for (var i = 0; i < Size; i++)
                        {
                            stringBuilder.Append(matrixes[k][i, j] ? "*" : "O");
                        }
                    }
                }
                break;
            case CubeView.Right:
                for (var j = Size - 1; j >= 0; j--)
                {
                    for (var k = 0; k < Size; k++)
                    {
                        for (var i = Size - 1; i >= 0; i--)
                        {
                            stringBuilder.Append(matrixes[k][i, j] ? "*" : "O");
                        }
                    }
                }
                break;
            case CubeView.Front:
                for (var i = Size - 1; i >= 0; i--)
                {
                    for (var k = 0; k < Size; k++)
                    {
                        for (var j = 0; j < Size; j++)
                        {
                            stringBuilder.Append(matrixes[k][i, j] ? "*" : "O");
                        }
                    }
                }
                break;
            case CubeView.Back:
                for (var i = 0; i < Size; i++)
                {
                    for (var k = 0; k < Size; k++)
                    {
                        for (var j = Size - 1; j >= 0; j--)
                        {
                            stringBuilder.Append(matrixes[k][i, j] ? "*" : "O");
                        }
                    }
                }
                break;
        }

        return stringBuilder.ToString();
    }

    public void OutputCubes()
    {
        var n = 1;
        foreach (var cubeView in CubeViews.OrderBy(v => v))
        {
            DataOutputWriter.OutputCube(cubeView, n, Size);
            n++;
        }
    }
}
