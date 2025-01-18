namespace PFunctionAnalyzerAlg.Helpers;

internal static class CombinationsHelper
{
    public static List<(byte x, byte y)[]> GetAllPossibleCombinations(byte size, byte elements)
    {
        var allPositions = new (byte x, byte y)[size * size];
        int index = 0;
        for (byte x = 0; x < size; x++)
        {
            for (byte y = 0; y < size; y++)
            {
                allPositions[index++] = (x, y);
            }
        }

        var result = new List<(byte x, byte y)[]>();
        GenerateCombinations(allPositions, elements, 0, 0, new (byte x, byte y)[elements], result);
        return result;
    }

    private static void GenerateCombinations((byte x, byte y)[] items, byte count, int startIndex, int depth, (byte x, byte y)[] combination, List<(byte x, byte y)[]> result)
    {
        if (depth == count)
        {
            var res = new (byte x, byte y)[combination.Length];
            Array.Copy(combination, res, combination.Length);
            result.Add(res);
            return;
        }

        for (var i = startIndex; i <= items.Length - (count - depth); i++)
        {
            combination[depth] = items[i];
            GenerateCombinations(items, count, i + 1, depth + 1, combination, result);
        }
    }
}
