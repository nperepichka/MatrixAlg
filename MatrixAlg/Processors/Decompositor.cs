using MatrixAlg.Helpers;
using MatrixShared.Models;

namespace MatrixAlg.Processors;

internal class Decompositor(bool[,] Input, byte Transversal)
{
    private readonly byte Size = (byte)Input.GetLength(0);
    private readonly byte ParallelBeforeIndex = (byte)(Input.GetLength(0) - 1);
    private readonly List<byte[]> InputPositionsPerRow = [];

    private readonly int MaxParallels = ParallelsConfiguration.MaxParallels;
    private readonly ParallelOptions ParallelOptionsMax = new() { MaxDegreeOfParallelism = ParallelsConfiguration.MaxParallels };
    private readonly ParallelOptions ParallelOptions = new() { MaxDegreeOfParallelism = 2 };
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

        for (byte i = 0; i < Size; i++)
        {
            var canUse = true;

            for (byte j = 0; j < Size; j++)
            {
                if (matrixIndex == j)
                {
                    for (byte e = 0; e < decomposition[j].Length; e++)
                    {
                        // current matrix of current decomposition does not contain this column
                        if (decomposition[j][e] == InputPositionsPerRow[row][i])
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
                if (decomposition[j][row] == InputPositionsPerRow[row][i])
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

                newDecomposition[matrixIndex][row] = InputPositionsPerRow[row][i];

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
                        newDecomposition.WriteDecomposition(DecomposesCount);

                        /*if (Size > 6 && DecomposesCount % 1000000 == 0)
                        {
                            Console.WriteLine($"Processed: {DecomposesCount}");
                        }*/
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
                if (ParallelsCount < MaxParallels && i1 < ParallelBeforeIndex)
                {
                    Interlocked.Increment(ref ParallelsCount);
                    Parallel.For(i1, Size, ParallelOptions, i2 =>
                    {
                        ValidateAndGenerateDecompositionMatrixNextRowVariants(row, decomposition, InputPositionsPerRow[row][i2]);
                    });
                    Interlocked.Decrement(ref ParallelsCount);
                    return;
                }

                ValidateAndGenerateDecompositionMatrixNextRowVariants(row, decomposition, InputPositionsPerRow[row][i1]);
            }
        }
        else
        {
            Interlocked.Add(ref ParallelsCount, Size);
            Parallel.For(0, Size, ParallelOptionsMax, i =>
            {
                ValidateAndGenerateDecompositionMatrixNextRowVariants(row, decomposition, InputPositionsPerRow[row][i]);
                Interlocked.Decrement(ref ParallelsCount);
            });
        }
    }
}
