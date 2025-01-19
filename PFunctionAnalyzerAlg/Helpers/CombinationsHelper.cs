namespace PFunctionAnalyzerAlg.Helpers;

internal static class CombinationsHelper
{
    public static IEnumerable<(byte x, byte y)[]> GetAllPossibleCombinations(byte size, byte elements)
    {
        // TODO: fix required, for size=17 or more result has some wrong values (looks like only 256 (aka byte) values will be filled)

        var totalPositions = size * size;
        var allPositions = new (byte x, byte y)[totalPositions];
        for (byte x = 0, index = 0; x < size; x++)
        {
            for (byte y = 0; y < size; y++)
            {
                allPositions[index] = (x, y);
                index++;
            }
        }

        var maxIndexBase = totalPositions - elements;
        var combination = new (byte x, byte y)[elements];

        IEnumerable<(byte x, byte y)[]> GenerateCombinations(int startIndex, int depth)
        {
            var nextDepth = depth + 1;
            var maxIndex = maxIndexBase + nextDepth;
            if (maxIndex == totalPositions)
            {
                for (var i = startIndex; i < maxIndex; i++)
                {
                    combination[depth] = allPositions[i];
                    var res = new (byte x, byte y)[elements];
                    Array.Copy(combination, res, elements);
                    yield return res;
                }
            }
            else
            {
                for (var i = startIndex; i < maxIndex; i++)
                {
                    combination[depth] = allPositions[i];
                    foreach (var subCombination in GenerateCombinations(i + 1, nextDepth))
                    {
                        yield return subCombination;
                    }
                }
            }
        }

        return GenerateCombinations(0, 0);
    }
}
