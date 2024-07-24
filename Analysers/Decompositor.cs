namespace MatrixAlg.Analysers;

internal class Decompositor(bool[,] Input, int Transversal, Action<IEnumerable<int[]>> OutputDecompose) : IDisposable
{
    private readonly int Size = Input.GetLength(0);
    private readonly List<int[]> InputPositionsPerRow = [];

    public void Decompose()
    {
        GenerateInputPositionsPerRow();

        var initialDecompositionSytate = BuildDecompositionWithFirstRow();
        GenerateDecompositions(1, initialDecompositionSytate);
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

    private void GenerateDecompositions(int n, int[][] decomposition)
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
        foreach (var nextDecomposition in decompositions)
        {
            if (n == Size)
            {
                OutputDecompose(decomposition);
            }
            else
            {
                GenerateDecompositions(n, nextDecomposition);
            }
        }
    }

    private static int[][] CloneDecomposition(int[][] original, bool addRow)
    {
        var clone = new int[original.Length][];
        for (var i = 0; i < original.Length; i++)
        {
            var length = original[i].Length;
            clone[i] = new int[addRow ? length + 1 : length];
            for (var j = 0; j < length; j++)
            {
                clone[i][j] = original[i][j];
            }
            if (addRow)
            {
                clone[i][length] = -1;
            }
        }
        return clone;
    }

    public void Dispose()
    {
        InputPositionsPerRow.Clear();
    }
}
