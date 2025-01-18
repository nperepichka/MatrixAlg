using MatrixShared.Helpers;
using PFunctionAnalyzerAlg.Helpers;
using PFunctionAnalyzerAlg.Models;
using System.Diagnostics;

namespace PFunctionAnalyzerAlg;

internal class Program
{
    private static byte Size = 0;
    private static byte Zeros = 0;

    private static void Main()
    {
        Size = ConsoleInputReader.ReadValue();
        Zeros = ConsoleInputReader.ReadValue(nameof(Zeros), Size);

        var stopwatch = Stopwatch.StartNew();

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
                    AddResultMatrix(pResMax, matrix, true);
                }
                else if (pFunc == pValMax)
                {
                    AddResultMatrix(pResMax, matrix);
                }

                if (pFunc < pValMin)
                {
                    pValMin = pFunc;
                    AddResultMatrix(pResMin, matrix, true);
                }
                else if (pFunc == pValMin)
                {
                    AddResultMatrix(pResMin, matrix);
                }

                if (pPlusFunc > pPlusValMax)
                {
                    pPlusValMax = pPlusFunc;
                    AddResultMatrix(pPlusResMax, matrix, true);
                }
                else if (pPlusFunc == pPlusValMax)
                {
                    AddResultMatrix(pPlusResMax, matrix);
                }

                if (pPlusFunc < pPlusValMin)
                {
                    pPlusValMin = pPlusFunc;
                    AddResultMatrix(pPlusResMin, matrix, true);
                }
                else if (pPlusFunc == pPlusValMin)
                {
                    AddResultMatrix(pPlusResMin, matrix);
                }
            }

            Console.WriteLine($"p min: {pValMin}");
            PrintResultMatrixes(pResMin);

            Console.WriteLine($"p max: {pValMax}");
            PrintResultMatrixes(pResMax);

            Console.WriteLine($"p+ min: {pPlusValMin}");
            PrintResultMatrixes(pPlusResMin);

            Console.WriteLine($"p+ max: {pPlusValMax}");
            PrintResultMatrixes(pPlusResMax);
        }

        stopwatch.Stop();
        Console.WriteLine();
        Console.WriteLine($"Processing elapsed in {stopwatch.ElapsedMilliseconds * 0.001:0.00}s");
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

    private static void AddResultMatrix(List<int[,]> res, int[,] matrix, bool clear = false)
    {
        if (ApplicationConfiguration.OutputMatrixes)
        {
            if (clear)
            {
                res.Clear();
            }
            res.Add(matrix);
        }
    }

    private static void PrintResultMatrixes(List<int[,]> matrixes)
    {
        if (ApplicationConfiguration.OutputMatrixes)
        {
            foreach (var matrix in matrixes)
            {
                PrintMatrix(matrix);
                Console.WriteLine();
            }
        }
    }

    private static void PrintMatrix(int[,] matrix, bool widePaddings = false)
    {
        for (byte i = 0; i < Size; i++)
        {
            for (byte j = 0; j < Size; j++)
            {
                var output = widePaddings
                    ? $" {matrix[i, j]}".PadLeft(3)
                    : $" {matrix[i, j]}";
                Console.Write(output);
            }
            Console.WriteLine();
        }
    }
}