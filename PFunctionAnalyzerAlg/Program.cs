using MatrixShared.Helpers;
using PFunctionAnalyzerAlg.Helpers;
using PFunctionAnalyzerAlg.Models;
using System.Diagnostics;

namespace PFunctionAnalyzerAlg;

internal class Program
{
    private static byte Size = 0;
    private static byte MaxAmountOfZero = 0;
    private static byte MaxAmountOfMinusOne = 0;

    private static int PrimayElement = 0;
    private static int SecondaryElement = -1;
    private static byte PrimayMax = 0;
    private static byte SecondaryMax = 0;

    private static readonly ParallelOptions ParallelOptionsMax = new() { MaxDegreeOfParallelism = ApplicationConfiguration.MaxParallelization };
    private static readonly object Lock = new();

    private static void Main()
    {
        Size = ConsoleInputReader.ReadValue();
        var max = Math.Min(255, Size * Size);
        MaxAmountOfZero = ConsoleInputReader.ReadValue(nameof(MaxAmountOfZero), (byte)max, true);
        MaxAmountOfMinusOne = ConsoleInputReader.ReadValue(nameof(MaxAmountOfMinusOne), (byte)(max - MaxAmountOfZero), true);

        if (MaxAmountOfZero == 0 && MaxAmountOfMinusOne == 0)
        {
            Console.WriteLine();
            Console.WriteLine("Nothing to process. Press <Enter> to exit...");
            Console.ReadLine();
            return;
        }

        if (MaxAmountOfMinusOne > MaxAmountOfZero)
        {
            PrimayElement = -1;
            SecondaryElement = 0;
            PrimayMax = MaxAmountOfMinusOne;
            SecondaryMax = MaxAmountOfZero;
        }
        else
        {
            PrimayMax = MaxAmountOfZero;
            SecondaryMax = MaxAmountOfMinusOne;
        }

        var stopwatch = Stopwatch.StartNew();

        var partitions = PFunctionsHelper.CalculatePartitions(Size);
        var primaryCombinations = CombinationsHelper.GetAllPossibleCombinations(Size, PrimayMax, []);

        if (MaxAmountOfZero == 1 && MaxAmountOfMinusOne == 0)
        {
            // Only calculate gravity matrix

            var pRes = GetMatrix([], []);
            var pPlusRes = GetMatrix([], []);

            Parallel.ForEach(primaryCombinations, ParallelOptionsMax, combination =>
            {
                var matrix = GetMatrix(combination, []);
                var (pFunc, pPlusFunc) = matrix.CalculatePFunctions(partitions, Size);
                pRes[combination[0].X, combination[0].Y] = pFunc;
                pPlusRes[combination[0].X, combination[0].Y] = pPlusFunc;
            });

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

            Parallel.ForEach(primaryCombinations, ParallelOptionsMax, pCombination =>
            {
                var secondaryCombinations = SecondaryMax != 0
                    ? CombinationsHelper.GetAllPossibleCombinations(Size, SecondaryMax, pCombination)
                    : [[]];

                foreach (var sCombination in secondaryCombinations)
                {
                    var matrix = GetMatrix(pCombination, sCombination);
                    var (pFunc, pPlusFunc) = matrix.CalculatePFunctions(partitions, Size);

                    Monitor.Enter(Lock);

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

                    Monitor.Exit(Lock);
                }
            });

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
        Console.WriteLine("Done. Press <Enter> to exit...");
        Console.ReadLine();
    }

    private static int[,] GetMatrix(Element[] primaryElements, Element[] secondaryElements)
    {
        var res = new int[Size, Size];

        for (byte i = 0; i < Size; i++)
        {
            for (byte j = 0; j < Size; j++)
            {
                res[i, j] = 1;
            }
        }

        foreach (var element in primaryElements)
        {
            res[element.X, element.Y] = PrimayElement;
        }
        foreach (var element in secondaryElements)
        {
            res[element.X, element.Y] = SecondaryElement;
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