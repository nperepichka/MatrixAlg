using MatrixShared.Analyzers;

namespace MatrixAlg.Models;

public class DecompositionMatrixDetails
{
    public DecompositionMatrixDetails(byte[] matrix, int index)
    {
        Index = index;
        Matrix = BuildMatrix(matrix);
        IsSymetric = Matrix.IsSymetric();
    }

    private static bool[,] BuildMatrix(byte[] matrix)
    {
        var size = matrix.Length;
        var res = new bool[size, size];

        for (var i = 0; i < size; i++)
        {
            res[i, matrix[i]] = true;
        }

        return res;
    }

    public bool[,] Matrix { get; set; }
    public int Index { get; set; }
    public bool IsSymetric { get; set; }
}
