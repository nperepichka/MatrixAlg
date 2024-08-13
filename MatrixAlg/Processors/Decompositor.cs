using MatrixAlg.Helpers;

namespace MatrixAlg.Processors;

internal class Decompositor(bool[,] Input, byte Transversal, CubeCreator CubeCreator)
{
    private readonly byte Size = (byte)Input.GetLength(0);
    private readonly List<byte[]> InputPositionsPerRow = [];
    private readonly ParallelOptions ParallelOptions = new() { MaxDegreeOfParallelism = -1 };
    private readonly int ExpectedParallelsCount = Environment.ProcessorCount / 4;
    private int ParallelsCount = 0;

    /// <summary>
    /// Decomposition counter
    /// </summary>
    public ulong DecomposesCount = 0;

    public void Decompose()
    {
        GenerateInputPositionsPerRow();

        var initialDecompositionState = BuildDecompositionWithFirstRow();
        GenerateDecompositions(1, initialDecompositionState);
    }

    private void GenerateInputPositionsPerRow()
    {
        for (byte i = 0; i < Size; i++)
        {
            var elementsForLine = new List<byte>(Transversal);
            for (byte j = 0; j < Size; j++)
            {
                if (Input[i, j])
                {
                    elementsForLine.Add(j);
                }
            }
            InputPositionsPerRow.Add([.. elementsForLine]);
        }
    }

    private byte[][] BuildDecompositionWithFirstRow()
    {
        var decomposition = new byte[Transversal][];
        for (byte i = 0; i < Transversal; i++)
        {
            decomposition[i] = [InputPositionsPerRow[0][i]];
        }
        return decomposition;
    }

    private void GenerateDecompositionMatrixNextRowVariants(byte row, byte[][] decomposition, byte matrixIndex)
    {
        var nextMatrixIndex = matrixIndex;
        nextMatrixIndex++;

        var nextRow = row;
        nextRow++;

        foreach (var nextRowVariant in InputPositionsPerRow[row])
        {
            if (
                !decomposition[matrixIndex].Contains(nextRowVariant) && // current matrix of current decomposition does not contain this column
                decomposition.All(m => m[row] != nextRowVariant) // all matrixes of current decomposition does not contain this column in this row
                )
            {
                var newDecomposition = CloneDecomposition(decomposition, false);
                newDecomposition[matrixIndex][row] = nextRowVariant;

                if (nextMatrixIndex != Transversal)
                {
                    GenerateDecompositionMatrixNextRowVariants(row, newDecomposition, nextMatrixIndex);
                }
                else
                {
                    if (nextRow != Size)
                    {
                        GenerateDecompositions(nextRow, newDecomposition);
                    }
                    else
                    {
                        // Increase decompositions count
                        Interlocked.Increment(ref DecomposesCount);
                        // Output decomposition
                        DataOutputWriter.WriteDecomposition(newDecomposition, DecomposesCount, CubeCreator);

                        /*if (Size > 6 && DecomposesCount % 1000000 == 0)
                        {
                            Console.WriteLine($"Processed: {DecomposesCount}");
                        }*/
                    }
                }
            }
        }
    }

    private void GenerateDecompositions(byte row, byte[][] decomposition)
    {
        // Build next row for 1st matrix

        if (ParallelsCount >= ExpectedParallelsCount || row % 2 == 0)
        {
            foreach (var nextRowVariant in InputPositionsPerRow[row])
            {
                if (!decomposition[0].Contains(nextRowVariant))
                {
                    var newDecomposition = CloneDecomposition(decomposition, true);
                    newDecomposition[0][row] = nextRowVariant;
                    GenerateDecompositionMatrixNextRowVariants(row, newDecomposition, 1);
                }
            }
        }
        else
        {
            Interlocked.Add(ref ParallelsCount, Transversal);
            Parallel.ForEach(InputPositionsPerRow[row], ParallelOptions, nextRowVariant =>
            {
                if (!decomposition[0].Contains(nextRowVariant))
                {
                    var newDecomposition = CloneDecomposition(decomposition, true);
                    newDecomposition[0][row] = nextRowVariant;
                    GenerateDecompositionMatrixNextRowVariants(row, newDecomposition, 1);
                }
                Interlocked.Decrement(ref ParallelsCount);
            });
        }
    }

    private static byte[][] CloneDecomposition(byte[][] original, bool addRow)
    {
        var currentRowsCount = original[0].Length;
        var newRowsCount = addRow ? currentRowsCount + 1 : currentRowsCount;

        var clone = new byte[original.Length][];

        for (var i = 0; i < original.Length; i++)
        {
            clone[i] = new byte[newRowsCount];
            Array.Copy(original[i], clone[i], currentRowsCount);
            if (addRow)
            {
                clone[i][currentRowsCount] = byte.MaxValue;
            }
        }

        return clone;
    }
}
