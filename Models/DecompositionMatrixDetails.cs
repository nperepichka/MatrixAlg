using MatrixAlg.Analysers;

namespace MatrixAlg.Models;

internal class DecompositionMatrixDetails
{
    public DecompositionMatrixDetails(byte[] matrixElements, int index)
    {
        Index = index;
        Matrix = BuildMatrix(matrixElements);
        IsSymetric = MatrixSymetricDetector.IsSymetric(Matrix);
    }

    public bool[,] Matrix { get; set; }
    public int Index { get; set; }
    public bool IsSymetric { get; set; }

    private static bool[,] BuildMatrix(byte[] elements)
    {
        var size = elements.Length;
        var res = new bool[size, size];
        for (var i = 0; i < size; i++)
        {
            for (var j = 0; j < size; j++)
            {
                res[i, j] = elements[i] == j;
            }
        }
        return res;
    }
}
