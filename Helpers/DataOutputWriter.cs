using MatrixAlg.Analysers;
using MatrixAlg.Models;
using System.Text;

namespace MatrixAlg.Helpers;

/// <summary>
/// Helper class, used to output data
/// </summary>
internal static class DataOutputWriter
{
    /// <summary>
    /// Write matrix
    /// </summary>
    /// <param name="matrix">Matrix</param>
    /// <param name="stringBuilder">Optional, string builder to use instead of default output</param>
    public static void WriteMatrix(bool[,] matrix, StringBuilder? stringBuilder = null)
    {
        // Get matrix size
        var size = matrix.GetLength(0);

        // Enumerate rows
        for (var i = 0; i < size; i++)
        {
            // Enumerate columns
            for (var j = 0; j < size; j++)
            {
                // Prepare cell value
                var cellValue = matrix[i, j] ? " *" : " O";
                // Check if string builder is not passed
                if (stringBuilder == null)
                {
                    // Write cell value to default output
                    OutputWriter.Write(cellValue);
                }
                // Else, if string builder is passed
                else
                {
                    // Append cell value to string builder
                    stringBuilder.Append(cellValue);
                }
            }
            // Check if string builder is not passed
            if (stringBuilder == null)
            {
                // Write empty line to default output
                OutputWriter.WriteLine();
            }
            else
            {
                // Else, append empty line to string builder
                stringBuilder.AppendLine();
            }
        }
    }

    /// <summary>
    /// Write decomposition output
    /// </summary>
    /// <param name="decomposition">Decompositions on 1-transversals</param>
    /// <param name="n">Number of decomposition</param>
    public static void WriteDecomposition(byte[][] decomposition, ulong n)
    {
        if (!ApplicationConfiguration.OutputDecompositions && !ApplicationConfiguration.DrawMosaics)
        {
            return;
        }

        // Define a list with matrixes details
        var matrixes = decomposition
            .Select((matrixElements, index) => new DecompositionMatrixDetails(matrixElements, index))
            .ToArray();

        if (ApplicationConfiguration.OutputDecompositions)
        {
            // Initiate string builder
            var outputStringBuilder = new StringBuilder(string.Empty);

            // Append decomposition counter value to string builder
            outputStringBuilder.AppendLine($"Decomposition #{n}");

            // Check if decomposition cube should be analyzed
            var shouldAnalyzeCube = ApplicationConfiguration.AnalyzeCubes && decomposition.Length == decomposition[0].Length;

            // If decomposition cube should be analyzed
            if (shouldAnalyzeCube)
            {
                // Check if cube of decomposition is symetric
                var isCubeSymetric = CubeSymetricDetector.IsSymetric(matrixes, out var sortedMartixes);
                // Set matrixes value
                matrixes = sortedMartixes;
                // If cube is symetric
                if (isCubeSymetric)
                {
                    // Append message that cube of decomposition is symetric to string builder
                    outputStringBuilder.AppendLine("Cube of decomposition is symetric.");
                }
                // If cube is not symetric
                else
                {
                    // Append message that cube of decomposition is not symetric to string builder
                    outputStringBuilder.AppendLine("Cube of decomposition is not symetric.");
                }
            }

            // Enumerate all matrixes
            foreach (var matrix in matrixes)
            {
                // Output matrix to string builder
                WriteMatrix(matrix.Matrix, outputStringBuilder);

                // If is symetric
                if (matrix.IsSymetric)
                {
                    // Append message that matrix is symetric to string builder
                    outputStringBuilder.AppendLine("Matrix is symetric.");
                }
                // If is self conjugate
                else if (MatrixСonjugationDetector.IsSelfConjugate(matrix.Matrix))
                {
                    // Append message that matrix is self conjugate to string builder
                    outputStringBuilder.AppendLine("Matrix is self conjugate.");
                }
                // If is not symetric and not self conjugate
                else
                {
                    // Check if conjugate not symetric matrix exists
                    var hasСonjugate = matrixes.Any(m => m.Index != matrix.Index && !m.IsSymetric && MatrixСonjugationDetector.AreСonjugate(matrix.Matrix, m.Matrix));
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

                        // TODO: instead of this test combined matrix (pending more details)
                    }
                }
            }

            // Write string builder value
            OutputWriter.WriteLine(outputStringBuilder.ToString());
        }

        // If decomposition mosaic should be drawn
        if (ApplicationConfiguration.DrawMosaics)
        {
            var mosaic = MosaicBuilder.BuildMosaic(matrixes);
            MosaicDrawer.Draw(mosaic, $"mosaic_{n}");
        }
    }
}
