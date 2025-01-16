using MatrixShared.Helpers;
using PFunctionAnalyzerAlg.Helpers;

namespace PFunctionAnalyzerAlg;

internal class Program
{
    private static byte Size = 0;
    private static byte Zeros = 0;

    private static void Main()
    {
        Size = ConsoleInputReader.ReadValue();
        Zeros = ConsoleInputReader.ReadValue(nameof(Zeros), Size);

        var partitions = PFunctionsHelper.CalculatePartitions(Size);
        var combinations = CombinationsHelper.GetAllPossibleCombinations(Size, Zeros);

        if (Zeros == 1)
        {
            var pRes = GetMatrix([]);
            var pPlusRes = GetMatrix([]);

            foreach (var combination in combinations)
            {
                var matrix = GetMatrix(combination);
                var (pFunc, pPlusFunc) = PFunctionsHelper.CalculatePFunctions(matrix, partitions);
                pRes[combination[0].x, combination[0].y] = pFunc;
                pPlusRes[combination[0].x, combination[0].y] = pPlusFunc;
            }

            Console.WriteLine("Gravity matrix (p):");
            PrintMatrix(pRes);

            Console.WriteLine();
            Console.WriteLine("Gravity matrix (p+):");
            PrintMatrix(pPlusRes);
        }
        else
        {
            // TODO: implement
        }
    }

    private static int[,] GetMatrix((byte x, byte y)[] zeros)
    {
        var res = new int[Size, Size];

        for (byte i = 0; i < Size; i++)
        {
            for (byte j = 0; j < Size; j++)
            {
                res[i, j] = 1;
            }
        }

        foreach (var (x, y) in zeros)
        {
            res[x, y] = 0;
        }

        return res;
    }

    private static void PrintMatrix(int[,] matrix)
    {
        for (byte i = 0; i < Size; i++)
        {
            for (byte j = 0; j < Size; j++)
            {
                Console.Write($" {matrix[i, j]}".PadLeft(3));
            }
            Console.WriteLine();
        }
    }

}