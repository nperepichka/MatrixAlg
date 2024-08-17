using MosaicAlg.Helpers;

namespace MosaicAlg.Processors;

internal class Decompositor(byte Size)
{
    private readonly ParallelOptions ParallelOptions = new() { MaxDegreeOfParallelism = -1 };
    private readonly int ExpectedParallelsCount = Environment.ProcessorCount / 4;
    private int ParallelsCount = 0;
    private readonly byte HalfSize = (byte)(Size / 2);

    public void Decompose()
    {
        var initialDecompositionState = new byte[HalfSize][];
        for (byte i = 0; i < HalfSize; i++)
        {
            initialDecompositionState[i] = [];
        }

        GenerateDecompositions(0, initialDecompositionState);
    }

    private void GenerateDecompositionMatrixNextRowVariants(byte row, byte[][] decomposition, byte matrixIndex)
    {
        var nextMatrixIndex = matrixIndex;
        nextMatrixIndex++;

        var nextRow = row;
        nextRow++;

        for (byte index = 0; index < Size; index++)
        {
            if (
                !decomposition[matrixIndex].Contains(index) && // current matrix of current decomposition does not contain this column
                decomposition.All(m => m[row] != index) // all matrixes of current decomposition does not contain this column in this row
                )
            {
                var newDecomposition = CloneDecomposition(decomposition, false);
                newDecomposition[matrixIndex][row] = index;

                if (nextMatrixIndex != HalfSize)
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
                        MosaicDrawer.Draw(newDecomposition, Size);
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
            for (byte index = 0; index < Size; index++)
            {
                if (!decomposition[0].Contains(index))
                {
                    var newDecomposition = CloneDecomposition(decomposition, true);
                    newDecomposition[0][row] = index;
                    GenerateDecompositionMatrixNextRowVariants(row, newDecomposition, 1);
                }
            }
        }
        else
        {
            Interlocked.Add(ref ParallelsCount, Size);
            Parallel.For(0, Size, ParallelOptions, i =>
            {
                var index = (byte)i;
                if (!decomposition[0].Contains(index))
                {
                    var newDecomposition = CloneDecomposition(decomposition, true);
                    newDecomposition[0][row] = index;
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
