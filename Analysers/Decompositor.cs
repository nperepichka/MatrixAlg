namespace MatrixAlg.Analysers
{
    internal class Decompositor(bool[,] Input, int Transversal, Action<IEnumerable<bool[,]>> OutputDecompose) : IDisposable
    {
        private readonly int Size = Input.GetLength(0);
        private readonly List<int[]> InputElementIndexesPerLine = [];
        private readonly List<int[]> ElementIndexesForAllTransversals = [];
        private readonly List<string> GeneratedHashes = [];

        public void Decompose()
        {
            GenerateInputElementIndexesPerLine();
            GenerateElementIndexesForAllTransversals(0, new int[Size]);
            GenerateDecompositions(0, []);
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

        private void GenerateDecompositions(int n, List<int[]> current)
        {
            if (n == Transversal)
            {
                var hash = CalculateHash(current);
                if (GeneratedHashes.Contains(hash))
                {
                    return;
                }

                GeneratedHashes.Add(hash);
                OutputDecompose(current.Select(BuildMatrix));

                return;
            }

            var variants = ElementIndexesForAllTransversals.Where(indexes =>
            {
                for (var i = 0; i < n; i++)
                {
                    for (var j = 0; j < Size; j++)
                    {
                        if (current[i][j] == indexes[j])
                        {
                            return false;
                        }
                    }
                }
                return true;
            });

            foreach (var variant in variants)
            {
                var next = current.ToList();
                next.Add(variant);
                GenerateDecompositions(n + 1, next);
            }
        }

        private void GenerateElementIndexesForAllTransversals(int line, int[] current)
        {
            if (line == Size)
            {
                ElementIndexesForAllTransversals.Add(current);
                return;
            }

            bool canUse;
            foreach (var index in InputElementIndexesPerLine[line])
            {
                canUse = true;
                for (var i = 0; i < line; i++)
                {
                    if (current[i] == index)
                    {
                        canUse = false;
                        break;
                    }
                }

                if (canUse)
                {
                    var next = current.ToArray();
                    next[line] = index;
                    GenerateElementIndexesForAllTransversals(line + 1, next);
                }
            }
        }

        private bool[,] BuildMatrix(int[] indexes)
        {
            var res = new bool[Size, Size];
            for (var i = 0; i < Size; i++)
            {
                for (var j = 0; j < Size; j++)
                {
                    res[i, j] = indexes[i] == j;
                }
            }
            return res;
        }

        private static string CalculateHash(List<int[]> elements)
        {
            var length = $"{elements.Count}".Length;
            var lines = elements.Select(_ => string.Join(string.Empty, _.Select(__ => $"{__}".PadLeft(length, '0'))));
            return string.Join(string.Empty, lines.Order());
        }

        public void Dispose()
        {
            InputElementIndexesPerLine.Clear();
            ElementIndexesForAllTransversals.Clear();
            GeneratedHashes.Clear();
        }
    }
}
