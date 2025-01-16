namespace PFunctionAnalyzerAlg.Helpers;

internal static class CombinationsHelper
{
    public static List<(byte x, byte y)[]> GetAllPossibleCombinations(byte size, byte elements)
    {
        var allPositions = new List<(byte x, byte y)>();
        for (byte x = 0; x < size; x++)
        {
            for (byte y = 0; y < size; y++)
            {
                allPositions.Add((x, y));
            }
        }

        var combinations = GetCombinations(allPositions, elements);
        return combinations.Select(c => c.ToArray()).ToList();
    }

    private static IEnumerable<IEnumerable<T>> GetCombinations<T>(IEnumerable<T> items, byte count)
    {
        if (count == 0)
        {
            yield return Enumerable.Empty<T>();
            yield break;
        }

        var itemsList = items.ToList();
        var nextCount = (byte)(count - 1);

        for (byte i = 0; i < itemsList.Count; i++)
        {
            var current = itemsList[i];
            var remaining = itemsList.Skip(i + 1);

            foreach (var combination in GetCombinations(remaining, nextCount))
            {
                yield return new[] { current }.Concat(combination);
            }
        }
    }
}
