using MatrixAlg.Helpers;

namespace MatrixAlg.Analysers;

internal class Decompositor(bool[,] Input, int Transversal, bool ShowOutput) : IDisposable
{
    private readonly int Size = Input.GetLength(0);
    private readonly List<int[]> InputPositionsPerRow = [];

    /// <summary>
    /// Decomposition counter
    /// </summary>
    public long DecomposesCount = 0;

    private int ParallelsCount = 0;
    private readonly int ParallelsLimit = ShowOutput ? 0 : Environment.ProcessorCount / 4;

    public async Task Decompose()
    {
        GenerateInputPositionsPerRow();

        var initialDecompositionSytate = BuildDecompositionWithFirstRow();
        await GenerateDecompositions(1, initialDecompositionSytate);
    }

    private void GenerateInputPositionsPerRow()
    {
        for (var i = 0; i < Size; i++)
        {
            var elementsForLine = new List<int>(Transversal);
            for (var j = 0; j < Size; j++)
            {
                if (Input[i, j])
                {
                    elementsForLine.Add(j);
                }
            }
            InputPositionsPerRow.Add([.. elementsForLine]);
        }
    }

    private int[][] BuildDecompositionWithFirstRow()
    {
        var decomposition = new int[Transversal][];
        for (var i = 0; i < Transversal; i++)
        {
            decomposition[i] = [InputPositionsPerRow[0][i]];
        }
        return decomposition;
    }

    private async Task GenerateDecompositions(int n, int[][] decomposition)
    {
        var nextRowVariants = InputPositionsPerRow[n];

        var decompositions = new List<int[][]>();

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
        for (var i = 1; i < Transversal; i++)
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
                Interlocked.Add(ref DecomposesCount, decompositions.Count);
            }
        }
        else
        {
            //if (n == 2)
            if (ParallelsCount < ParallelsLimit)
            {
                Interlocked.Add(ref ParallelsCount, decompositions.Count);
                await Parallel.ForEachAsync(decompositions, async (nextDecomposition, _) =>
                {
                    await GenerateDecompositions(n, nextDecomposition);
                    Interlocked.Decrement(ref ParallelsCount);
                });
            }
            else
            {
                foreach (var nextDecomposition in decompositions)
                {
                    await GenerateDecompositions(n, nextDecomposition);
                }
            }
        }
    }

    private static int[][] CloneDecomposition(int[][] original, bool addRow)
    {
        var currentRowsCount = original[0].Length;
        var newRowsCount = addRow ? currentRowsCount + 1 : currentRowsCount;

        var clone = new int[original.Length][];

        for (var i = 0; i < original.Length; i++)
        {
            clone[i] = new int[newRowsCount];
            Array.Copy(original[i], clone[i], currentRowsCount);
            if (addRow)
            {
                clone[i][currentRowsCount] = -1;
            }
        }

        return clone;
    }

    public void Dispose()
    {
        InputPositionsPerRow.Clear();
    }
}
