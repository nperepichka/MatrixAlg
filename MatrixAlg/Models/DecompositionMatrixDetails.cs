using MatrixAlg.Analysers;

namespace MatrixAlg.Models;

internal class DecompositionMatrixDetails
{
    public DecompositionMatrixDetails(bool[,] matrix, int index)
    {
        Index = index;
        Matrix = matrix;
        IsSymetric = Matrix.IsSymetric();
    }

    public bool[,] Matrix { get; set; }
    public int Index { get; set; }
    public bool IsSymetric { get; set; }
}
