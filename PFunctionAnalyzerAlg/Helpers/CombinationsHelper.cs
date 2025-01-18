namespace PFunctionAnalyzerAlg.Helpers;

internal static class CombinationsHelper
{
    public static List<(byte x, byte y)[]> GetAllPossibleCombinations(byte size, byte elements)
    {
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
        var result = new List<(byte x, byte y)[]>(totalPositions);
        var combination = new (byte x, byte y)[elements];

        void GenerateCombinations(int startIndex, int depth)
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
                    result.Add(res);
                }
            }
            else
            {
                for (var i = startIndex; i < maxIndex; i++)
                {
                    combination[depth] = allPositions[i];
                    GenerateCombinations(i + 1, nextDepth);
                }
            }
        }

        GenerateCombinations(0, 0);
        return result;
    }
}
