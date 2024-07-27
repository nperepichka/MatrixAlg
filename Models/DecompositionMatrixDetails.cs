using MatrixAlg.Analysers;

namespace MatrixAlg.Models;

internal class DecompositionMatrixDetails
{
    public DecompositionMatrixDetails(byte[] matrixElements, int index)
    {
        MatrixElements = matrixElements;
        Index = index;
        Matrix = BuildMatrix(matrixElements);
        IsSymetric = MatrixSymetricDetector.IsSymetric(Matrix);
        IsSelfConjugate = MatrixСonjugationDetector.IsSelfConjugate(Matrix);
        HasСonjugate = false;
        Hash = GetHash(matrixElements);
    }

    public byte[] MatrixElements { get; set; }
    public bool[,] Matrix { get; set; }
    public int Index { get; set; }
    public bool IsSymetric { get; set; }
    public bool IsSelfConjugate { get; set; }
    public bool HasСonjugate { get; set; }
    public string Hash { get; set; }

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

    private static string GetHash(byte[] matrixElements)
    {
        var hashElementSize = matrixElements.Length.ToString().Length;
        return string.Concat(matrixElements.Select(e => e.ToString().PadLeft(hashElementSize)));
    }
}
