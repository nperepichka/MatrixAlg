using MatrixAlg.Helpers;

namespace MatrixAlg.Analysers;

internal class Decompositor(bool[,] Input, int Transversal, Action<IEnumerable<int[]>> OutputDecompose) : IDisposable
{
    private readonly int Size = Input.GetLength(0);
    private readonly List<int[]> InputElementIndexesPerLine = [];
    private readonly List<int[]> ElementIndexesForAllTransversals = [];
    private readonly ConcurrentHashSet<string> GeneratedHashes = new();

    private static int ParallelsCount = 0;
    private static readonly int ParallelsLimit = Environment.ProcessorCount / 4;

    public async Task Decompose()
    {
        GenerateInputElementIndexesPerLine();
        GenerateElementIndexesForAllTransversals(0, new int[Size]);
        await GenerateDecompositions(0, []);
    }

    private void GenerateInputElementIndexesPerLine()
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
            if (elementsForLine.Count != Transversal)
            {
                throw new Exception($"Invalid amount of elements in row - expected {Transversal} but found {elementsForLine.Count}.");
            }
            InputElementIndexesPerLine.Add([.. elementsForLine]);
        }
        if (InputElementIndexesPerLine.Count != Size)
        {
            throw new Exception($"Invalid amount of rows - expected {Size} but found {InputElementIndexesPerLine.Count}.");
        }
    }

    private async Task GenerateDecompositions(int n, List<int[]> current)
    {
        var i = 0;
        var j = 0;
        var variants = ElementIndexesForAllTransversals.Where(indexes =>
        {
            for (i = 0; i < n; i++)
            {
                for (j = 0; j < Size; j++)
                {
                    if (current[i][j] == indexes[j])
                    {
                        return false;
                    }
                }
            }
            return true;
        });

        if (ParallelsCount < ParallelsLimit)
        {
            await Parallel.ForEachAsync(variants, async (variant, _) =>
            {
                Interlocked.Increment(ref ParallelsCount);

                var next = current.ToList();
                next.Add(variant);
                if (n < Transversal - 2)
                {
                    await GenerateDecompositions(n + 1, next);
                }
                else
                {
                    GenerateLastDecomposition(next);
                }

                Interlocked.Decrement(ref ParallelsCount);
            });
        }
        else
        {
            foreach (var variant in variants)
            {
                var next = current.ToList();
                next.Add(variant);
                if (n < Transversal - 2)
                {
                    await GenerateDecompositions(n + 1, next);
                }
                else
                {
                    GenerateLastDecomposition(next);
                }
            }
        }
    }

    private void GenerateLastDecomposition(List<int[]> current)
    {
        var i = 0;
        var j = 0;
        var variant = ElementIndexesForAllTransversals.First(indexes =>
        {
            for (i = 0; i < Transversal - 1; i++)
            {
                for (j = 0; j < Size; j++)
                {
                    if (current[i][j] == indexes[j])
                    {
                        return false;
                    }
                }
            }
            return true;
        });

        current.Add(variant);
        var hash = CalculateHash(current);
        GeneratedHashes.Add(hash, () =>
        {
            OutputDecompose(current);
        });
    }

    private void GenerateElementIndexesForAllTransversals(int line, int[] current)
    {
        if (line == Size)
        {
            ElementIndexesForAllTransversals.Add(current);
            return;
        }

        foreach (var index in InputElementIndexesPerLine[line])
        {
            if (!current.Take(line).Contains(index))
            {
                var next = current.ToArray();
                next[line] = index;
                GenerateElementIndexesForAllTransversals(line + 1, next);
            }
        }
    }

    private string CalculateHash(List<int[]> elements)
    {
        var length = $"{Size - 1}".Length;
        return string.Concat(elements.Select(line => string.Concat(line.Select(index => $"{index}".PadLeft(length, '0')))).Order());
    }

    public void Dispose()
    {
        InputElementIndexesPerLine.Clear();
        ElementIndexesForAllTransversals.Clear();
        GeneratedHashes.Dispose();
        ParallelsCount = 0;
    }
}
