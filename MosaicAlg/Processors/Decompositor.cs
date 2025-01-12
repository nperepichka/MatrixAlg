using MosaicAlg.Helpers;
using MosaicAlg.Models;

namespace MosaicAlg.Processors;

internal class Decompositor(byte Size)
{
    private readonly byte HalfSize = (byte)(Size / 2);
    private readonly byte ParallelBeforeIndex = (byte)(Size - 1);

    private readonly ParallelOptions ParallelOptionsMax = new() { MaxDegreeOfParallelism = ApplicationConfiguration.MaxParallelization };
    private readonly ParallelOptions ParallelOptions = new() { MaxDegreeOfParallelism = 2 };
    private int ParallelsCount = 0;

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

            for (byte j = 0; j < HalfSize; j++)
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

        if (ParallelsCount != 0)
        {
            for (byte i1 = 0; i1 < Size; i1++)
            {
                if (ParallelsCount < ApplicationConfiguration.MaxParallelization && i1 < ParallelBeforeIndex)
                {
                    Interlocked.Increment(ref ParallelsCount);
                    Parallel.For(i1, Size, ParallelOptions, i2 =>
                    {
                        ValidateAndGenerateDecompositionMatrixNextRowVariants(row, decomposition, (byte)i2);
                    });
                    Interlocked.Decrement(ref ParallelsCount);
                    return;
                }

                ValidateAndGenerateDecompositionMatrixNextRowVariants(row, decomposition, i1);
            }
        }
        else
        {
            Interlocked.Add(ref ParallelsCount, Size);
            Parallel.For(0, Size, ParallelOptionsMax, i =>
            {
                ValidateAndGenerateDecompositionMatrixNextRowVariants(row, decomposition, (byte)i);
                Interlocked.Decrement(ref ParallelsCount);
            });
        }
    }
}
