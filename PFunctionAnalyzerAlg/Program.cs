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
            PrintMatrix(pRes, true);

            Console.WriteLine();
            Console.WriteLine("Gravity matrix (p+):");
            PrintMatrix(pPlusRes, true);
        }
        else
        {
            var pValMin = int.MaxValue;
            var pValMax = int.MinValue;
            var pPlusValMin = int.MaxValue;
            var pPlusValMax = int.MinValue;
            List<int[,]> pResMin = [];
            List<int[,]> pResMax = [];
            List<int[,]> pPlusResMin = [];
            List<int[,]> pPlusResMax = [];

            foreach (var combination in combinations)
            {
                var matrix = GetMatrix(combination);
                var (pFunc, pPlusFunc) = PFunctionsHelper.CalculatePFunctions(matrix, partitions);

                if (pFunc > pValMax)
                {
                    pValMax = pFunc;
                    pResMax = [matrix];
                }
                else if (pFunc == pValMax)
                {
                    pResMax.Add(matrix);
                }

                if (pFunc < pValMin)
                {
                    pValMin = pFunc;
                    pResMin = [matrix];
                }
                else if (pFunc == pValMin)
                {
                    pResMin.Add(matrix);
                }

                if (pPlusFunc > pPlusValMax)
                {
                    pPlusValMax = pPlusFunc;
                    pPlusResMax = [matrix];
                }
                else if (pPlusFunc == pPlusValMax)
                {
                    pPlusResMax.Add(matrix);
                }

                if (pPlusFunc < pPlusValMin)
                {
                    pPlusValMin = pPlusFunc;
                    pPlusResMin = [matrix];
                }
                else if (pPlusFunc == pPlusValMin)
                {
                    pPlusResMin.Add(matrix);
                }
            }

            Console.WriteLine($"p min: {pValMin}");
            foreach (var matrix in pResMin)
            {
                PrintMatrix(matrix);
                Console.WriteLine();
            }
            Console.WriteLine($"p max: {pValMax}");
            foreach (var matrix in pResMax)
            {
                PrintMatrix(matrix);
                Console.WriteLine();
            }
            Console.WriteLine($"p+ min: {pPlusValMin}");
            foreach (var matrix in pPlusResMin)
            {
                PrintMatrix(matrix);
                Console.WriteLine();
            }
            Console.WriteLine($"p+ max: {pPlusValMax}");
            foreach (var matrix in pPlusResMax)
            {
                PrintMatrix(matrix);
                Console.WriteLine();
            }
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

    private static void PrintMatrix(int[,] matrix, bool withPaddings = false)
    {
        for (byte i = 0; i < Size; i++)
        {
            for (byte j = 0; j < Size; j++)
            {
                if (withPaddings)
                {
                    Console.Write($" {matrix[i, j]}".PadLeft(3));
                }
                else
                {
                    Console.Write(matrix[i, j]);
                }
            }
            Console.WriteLine();
        }
    }

}