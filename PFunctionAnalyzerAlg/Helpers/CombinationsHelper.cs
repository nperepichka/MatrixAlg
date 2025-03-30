using PFunctionAnalyzerAlg.Models;

namespace PFunctionAnalyzerAlg.Helpers;

internal static class CombinationsHelper
{
    public static IEnumerable<Element[]> GetAllPossibleCombinations(byte size, byte elementsAmount, Element[] blockedElements)
    {
        if (elementsAmount == 0)
            return [];

        var totalPositions = size * size;
        var allPositions = new Element[totalPositions];

        var index = 0;
        for (byte x = 0; x < size; x++)
        {
            for (byte y = 0; y < size; y++)
            {
                allPositions[index++] = new Element
                {
                    X = x,
                    Y = y
                };
            }
        }

        var maxIndexBase = totalPositions - elementsAmount;
        var combination = new Element[elementsAmount];

        bool CanProcess(Element position)
        {
            if (blockedElements.Length == 0)
                return true;

            for (var j = 0; j < blockedElements.Length; j++)
            {
                if (blockedElements[j].X == position.X && blockedElements[j].Y == position.Y)
                    return false;
            }
            return true;
        }

        IEnumerable<Element[]> GenerateCombinations(int startIndex, int depth)
        {
            var nextDepth = depth + 1;
            var maxIndex = maxIndexBase + nextDepth;
            if (maxIndex == totalPositions)
            {
                for (var i = startIndex; i < maxIndex; i++)
                {
                    var position = allPositions[i];
                    var canProcess = CanProcess(position);
                    if (canProcess)
                    {
                        combination[depth] = position;
                        var res = new Element[elementsAmount];
                        Array.Copy(combination, res, elementsAmount);
                        yield return res;
                    }
                }
            }
            else
            {
                for (var i = startIndex; i < maxIndex; i++)
                {
                    var position = allPositions[i];
                    var canProcess = CanProcess(position);
                    if (canProcess)
                    {
                        combination[depth] = position;
                        foreach (var subCombination in GenerateCombinations(i + 1, nextDepth))
                        {
                            yield return subCombination;
                        }
                    }
                }
            }
        }

        return GenerateCombinations(0, 0);
    }
}
