using MatrixAlg.Analysers;
using MatrixAlg.Models;
using System.Text;

namespace MatrixAlg.Helpers;

/// <summary>
/// Helper class, used to output data
/// </summary>
public static class DataOutputWriter
{
    /// <summary>
    /// Write matrix
    /// </summary>
    /// <param name="matrix">Matrix</param>
    /// <param name="stringBuilder">Optional, string builder to use instead of default output</param>
    public static void WriteMatrix(this bool[,] matrix, StringBuilder stringBuilder)
    {
        // Get matrix size
        var size = matrix.GetLength(0);

        // Enumerate rows
        for (var i = 0; i < size; i++)
        {
            // Enumerate columns
            for (var j = 0; j < size; j++)
            {
                // Append cell value to string builder
                stringBuilder.Append(matrix[i, j] ? " *" : " O");
            }
            // Append empty line to string builder
            stringBuilder.AppendLine();
        }
    }

    public static void WriteDecomposition(this byte[][] decomposition, ulong n)
    {
        if (!ApplicationConfiguration.OutputDecompositions)
        {
            return;
        }

        // Define a list with matrixes details
        var matrixesWithDetailes = decomposition
            .Select((matrix, index) => new DecompositionMatrixDetails(matrix, index))
            .ToArray();

        // Initiate string builder
        var outputStringBuilder = new StringBuilder(string.Empty);

        // Append decomposition counter value to string builder
        outputStringBuilder.AppendLine($"Decomposition #{n}");

        // Enumerate all matrixes
        foreach (var matrix in matrixesWithDetailes)
        {
            // Output matrix to string builder
            matrix.Matrix.WriteMatrix(outputStringBuilder);

            // If is symetric
            if (matrix.IsSymetric)
            {
                // Append message that matrix is symetric to string builder
                outputStringBuilder.AppendLine("Matrix is symetric.");
            }
            // If is self conjugate
            else if (matrix.Matrix.IsSelfConjugate())
            {
                // Append message that matrix is self conjugate to string builder
                outputStringBuilder.AppendLine("Matrix is self conjugate.");
            }
            // If is not symetric and not self conjugate
            else
            {
                // Check if conjugate not symetric matrix exists
                var hasСonjugate = matrixesWithDetailes.Any(m => m.Index != matrix.Index && !m.IsSymetric && MatrixСonjugationDetector.AreСonjugate(matrix.Matrix, m.Matrix));
                // If conjugate not symetric matrix exists
                if (hasСonjugate)
                {
                    // Append message that matrix is conjugate to other matrix to string builder
                    outputStringBuilder.AppendLine("Matrix is conjugate to other matrix.");
                }
                // If conjugate not symetric matrix not exists
                else
                {
                    // Append message that matrix is not conjugate to other matrix to string builder
                    outputStringBuilder.AppendLine("Matrix is not symetric, not self conjugate and not conjugate to other matrix.");

                    // TODO: test combined matrix (pending more details)
                }
            }
        }

        // Write string builder value
        OutputWriter.WriteLine(outputStringBuilder.ToString());
    }
}
