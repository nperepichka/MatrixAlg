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

    public void CreateInvariantCubes(DecompositionMatrixDetails[] decomposition)
    {
        foreach (var combination in Combinations)
        {
            var matrixesCombination = combination
                .Select(i => decomposition[i].Matrix)
                .ToArray();

            var topViews = ViewVariantsToString(matrixesCombination, CubeView.Top);

            if (
                   topViews.Any(tv => tv == ViewToString(matrixesCombination, CubeView.Right))
                && topViews.Any(tv => tv == ViewToString(matrixesCombination, CubeView.Back))
                && topViews.Any(tv => tv == ViewToString(matrixesCombination, CubeView.Left))
                && topViews.Any(tv => tv == ViewToString(matrixesCombination, CubeView.Front))
                // no need to check bottom view
                //&& topViews.Any(tv => tv == ViewToString(matrixesCombination, CubeView.Bottom))
                )
            {
                var uniqueCandidateView = topViews.Min(tv => tv)!;
                bool isUnique;
                lock (CubeLock)
                {
                    isUnique = CubeViews.Add(uniqueCandidateView);
                }
                if (isUnique)
                {
                    var formatedView = FormatView(uniqueCandidateView);
                    OutputWriter.WriteLine($"Unique invariant cube found:{Environment.NewLine}{formatedView}{Environment.NewLine}");
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

    private List<string> ViewVariantsToString(bool[][,] matrixes, CubeView view)
    {
        var res = new List<string>(4)
        {
            ViewToString(matrixes, view)
        };

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
        int k;
        int i;
        int j;

        switch (view)
        {
            case CubeView.Top:
                for (k = 0; k < Size; k++)
                {
                    for (i = 0; i < Size; i++)
                    {
                        for (j = 0; j < Size; j++)
                        {
                            stringBuilder.Append(matrixes[k][i, j] ? '*' : 'O');
                        }
                    }
                }
                break;
            case CubeView.Right:
                for (j = Size - 1; j >= 0; j--)
                {
                    for (k = 0; k < Size; k++)
                    {
                        for (i = Size - 1; i >= 0; i--)
                        {
                            stringBuilder.Append(matrixes[k][i, j] ? '*' : 'O');
                        }
                    }
                }
                break;
            case CubeView.Back:
                for (i = 0; i < Size; i++)
                {
                    for (k = 0; k < Size; k++)
                    {
                        for (j = Size - 1; j >= 0; j--)
                        {
                            stringBuilder.Append(matrixes[k][i, j] ? '*' : 'O');
                        }
                    }
                }
                break;
            case CubeView.Left:
                for (j = 0; j < Size; j++)
                {
                    for (k = 0; k < Size; k++)
                    {
                        for (i = 0; i < Size; i++)
                        {
                            stringBuilder.Append(matrixes[k][i, j] ? '*' : 'O');
                        }
                    }
                }
                break;
            case CubeView.Front:
                for (i = Size - 1; i >= 0; i--)
                {
                    for (k = 0; k < Size; k++)
                    {
                        for ( j = 0; j < Size; j++)
                        {
                            stringBuilder.Append(matrixes[k][i, j] ? '*' : 'O');
                        }
                    }
                }
                break;
            case CubeView.Bottom:
                for (k = Size - 1; k >= 0; k--)
                {
                    for (i = Size - 1; i >= 0; i--)
                    {
                        for (j = 0; j < Size; j++)
                        {
                            stringBuilder.Append(matrixes[k][i, j] ? '*' : 'O');
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
