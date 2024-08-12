using MatrixAlg.Analysers;
using MatrixAlg.Models;
using MatrixAlg.Processors;
using System.Text;

namespace MatrixAlg.Helpers;

/// <summary>
/// Helper class, used to output data
/// </summary>
internal static class DataOutputWriter
{
    private static readonly object MosaicLock = new();

    private static readonly HashSet<string> MosaicHashes = [];

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
                if (stringBuilder is null)
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
            if (stringBuilder is null)
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

    public static void WriteDecomposition(byte[][] decomposition, ulong n, CubeCreator cubeCreator)
    {
        if (!ApplicationConfiguration.OutputDecompositions && !ApplicationConfiguration.AnalyzeCubes && !ApplicationConfiguration.DrawMosaics)
        {
            return;
        }

        var matrixes = decomposition
            .Select(MatrixBuilder.BuildMatrix)
            .ToArray();

        if (ApplicationConfiguration.OutputDecompositions)
        {
            // Define a list with matrixes details
            var matrixesWithDetailes = matrixes
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

                        // TODO: instead of this test combined matrix (pending more details)
                    }
                }
            }

            // Write string builder value
            OutputWriter.WriteLine(outputStringBuilder.ToString());
        }

        // If decomposition cube should be analyzed
        if (ApplicationConfiguration.AnalyzeCubes)
        {
            // Create invariant cubes of decomposition
            cubeCreator.CreateInvariantCubes(matrixes);
        }

        // If decomposition mosaic should be drawn
        if (ApplicationConfiguration.DrawMosaics)
        {
            DrawMosaic(matrixes, n);
        }
    }

    private static void DrawMosaic(bool[][,] matrixes, ulong n)
    {
        var mosaic = MosaicBuilder.BuildMosaic(matrixes);

        var hash = GetHash(mosaic);
        bool isNew;
        lock (MosaicLock)
        {
            isNew = MosaicHashes.Add(hash);
        }
        if (isNew)
        {
            MosaicDrawer.Draw(mosaic, $"mosaic_{n}");
        }
    }

    private static string GetHash(bool[,] mosaic)
    {
        var size = mosaic.GetLength(0);
        var flatArray = new char[(size * size + 15) / 16];

        var bitIndex = 0;
        for (var i = 0; i < size; i++)
        {
            for (var j = 0; j < size; j++)
            {
                if (mosaic[i, j])
                {
                    flatArray[bitIndex / 16] |= (char)(1 << (bitIndex % 16));
                }
                bitIndex++;
            }
        }

        return new string(flatArray);
    }

    public static void OutputCube(string cubeView, int n, byte size)
    {
        var stringBuilder = new StringBuilder($"Invarianat cube #{n}{Environment.NewLine}");

        var start = 0;
        while (start < cubeView.Length)
        {
            var slice = new List<int>(size);
            for (var i = 0; i < size; i++)
            {
                slice.Add(cubeView.Substring(start, size).IndexOf('*'));
                start += size;
            }
            stringBuilder.Append($"({string.Join(';', slice)}) ");
        }

        OutputWriter.WriteLine(stringBuilder.ToString());
    }
}
