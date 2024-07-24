using MatrixAlg.Helpers;

namespace MatrixAlg.Analysers;

internal class Decompositor(bool[,] Input, byte Transversal, bool ShowOutput)
{
    private readonly byte Size = (byte)Input.GetLength(0);
    private readonly List<byte[]> InputPositionsPerRow = [];
    private readonly byte ParallelOnRow = ShowOutput ? byte.MaxValue : (byte)2;
    private readonly ParallelOptions ParallelOptions = new() { MaxDegreeOfParallelism = -1 };

    /// <summary>
    /// Decomposition counter
    /// </summary>
    public ulong DecomposesCount = 0;

    public void Decompose()
    {
        GenerateInputPositionsPerRow();

        var initialDecompositionSytate = BuildDecompositionWithFirstRow();
        GenerateDecompositions(1, initialDecompositionSytate);
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

    private void GenerateDecompositions(byte n, byte[][] decomposition)
    {
        var nextRowVariants = InputPositionsPerRow[n];

        var decompositions = new List<byte[][]>();

        // Build next row for 1st matrix
        foreach (var nextRowVariant in nextRowVariants)
        {
            if (!decomposition[0].Contains(nextRowVariant))
            {
                var newDecomposition = CloneDecomposition(decomposition, true);
                newDecomposition[0][n] = nextRowVariant;
                decompositions.Add(newDecomposition);
            }
        }

        // Build next row for each other matrix
        for (byte i = 1; i < Transversal; i++)
        {
            var currentDecompositions = decompositions.ToArray();
            decompositions.Clear();

            // Take a decomposition
            foreach (var currentDecomposition in currentDecompositions)
            {
                foreach (var nextRowVariant in nextRowVariants)
                {
                    if (
                        !currentDecomposition[i].Contains(nextRowVariant) && // current matrix of current decomposition does not contain this column
                        currentDecomposition.All(m => m[n] != nextRowVariant) // all matrixes of current decomposition does not contain this column in this row
                        )
                    {
                        var newDecomposition = CloneDecomposition(currentDecomposition, false);
                        newDecomposition[i][n] = nextRowVariant;
                        decompositions.Add(newDecomposition);
                    }
                }
            }
        }

        n++;
        if (n == Size)
        {
            if (ShowOutput)
            {
                foreach (var nextDecomposition in decompositions)
                {
                    Interlocked.Increment(ref DecomposesCount);
                    ConsoleWriter.WriteDecomposition(nextDecomposition, DecomposesCount);
                }
            }
            else
            {
                Interlocked.Add(ref DecomposesCount, (ulong)decompositions.Count);
            }
        }
        else if (n != ParallelOnRow)
        {
            foreach (var nextDecomposition in decompositions)
            {
                GenerateDecompositions(n, nextDecomposition);
            }
        }
        else
        {
            Parallel.ForEach(decompositions, ParallelOptions, nextDecomposition =>
            {
                GenerateDecompositions(n, nextDecomposition);
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
