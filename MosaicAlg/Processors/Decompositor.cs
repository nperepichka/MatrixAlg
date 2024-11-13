using MatrixShared.Models;
using MosaicAlg.Helpers;

namespace MosaicAlg.Processors;

internal class Decompositor(byte Size)
{
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
            var canUse = true;

            for (byte j = 0; j < Size; j++)
            {
                if (matrixIndex == j)
                {
                    for (byte e = 0; e < decomposition[j].Length; e++)
                    {
                        // current matrix of current decomposition does not contain this column
                        if (decomposition[j][e] == index)
                        {
                            canUse = false;
                            break;
                        }
                    }
                    if (!canUse)
                    {
                        break;
                    }
                }
                // all matrixes of current decomposition does not contain this column in this row
                if (decomposition[j][row] == index)
                {
                    canUse = false;
                    break;
                }
            }

            if (canUse)
            {
                var rowsCount = decomposition[0].Length;
                var newDecomposition = new byte[decomposition.Length][];

                for (byte j = 0; j < decomposition.Length; j++)
                {
                    newDecomposition[j] = new byte[rowsCount];
                    Array.Copy(decomposition[j], newDecomposition[j], rowsCount);
                }

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

    private void ValidateAndGenerateDecompositionMatrixNextRowVariants(byte row, byte[][] decomposition, byte value)
    {
        var rowsCount = decomposition[0].Length;
        for (byte j = 0; j < decomposition[0].Length; j++)
        {
            if (decomposition[0][j] == value)
            {
                return;
            }
        }

        var newRowsCount = rowsCount + 1;
        var newDecomposition = new byte[decomposition.Length][];

        for (byte i = 0; i < decomposition.Length; i++)
        {
            newDecomposition[i] = new byte[newRowsCount];
            Array.Copy(decomposition[i], newDecomposition[i], rowsCount);
            newDecomposition[i][rowsCount] = i == 0 ? value : byte.MaxValue;
        }

        GenerateDecompositionMatrixNextRowVariants(row, newDecomposition, 1);
    }

    private void GenerateDecompositions(byte row, byte[][] decomposition)
    {
        // Build next row for 1st matrix

        if (ParallelsCount >= ParallelsConfiguration.MaxParallels || row % 2 == 0)
        {
            for (byte index = 0; index < Size; index++)
            {
                ValidateAndGenerateDecompositionMatrixNextRowVariants(row, decomposition, index);
            }
        }
        else
        {
            var parallelOptions = new ParallelOptions() { MaxDegreeOfParallelism = Environment.ProcessorCount };

            Parallel.For(0, Size, parallelOptions, index =>
            {
                Interlocked.Increment(ref ParallelsCount);
                ValidateAndGenerateDecompositionMatrixNextRowVariants(row, decomposition, (byte)index);
                Interlocked.Decrement(ref ParallelsCount);
            });
        }
    }
}
