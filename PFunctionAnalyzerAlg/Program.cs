using MatrixShared.Helpers;
using PFunctionAnalyzerAlg.Helpers;
using PFunctionAnalyzerAlg.Models;

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
                    if (ApplicationConfiguration.OutputMatrixes)
                    {
                        pResMax = [matrix];
                    }
                }
                else if (pFunc == pValMax && ApplicationConfiguration.OutputMatrixes)
                {
                    pResMax.Add(matrix);
                }

                if (pFunc < pValMin)
                {
                    pValMin = pFunc;
                    if (ApplicationConfiguration.OutputMatrixes)
                    {
                        pResMin = [matrix];
                    }
                }
                else if (pFunc == pValMin && ApplicationConfiguration.OutputMatrixes)
                {
                    pResMin.Add(matrix);
                }

                if (pPlusFunc > pPlusValMax)
                {
                    pPlusValMax = pPlusFunc;
                    if (ApplicationConfiguration.OutputMatrixes)
                    {
                        pPlusResMax = [matrix];
                    }
                }
                else if (pPlusFunc == pPlusValMax && ApplicationConfiguration.OutputMatrixes)
                {
                    pPlusResMax.Add(matrix);
                }

                if (pPlusFunc < pPlusValMin)
                {
                    pPlusValMin = pPlusFunc;
                    if (ApplicationConfiguration.OutputMatrixes)
                    {
                        pPlusResMin = [matrix];
                    }
                }
                else if (pPlusFunc == pPlusValMin && ApplicationConfiguration.OutputMatrixes)
                {
                    pPlusResMin.Add(matrix);
                }
            }

            // TODO: some refactoring required for matrix related code

            Console.WriteLine($"p min: {pValMin}");
            if (ApplicationConfiguration.OutputMatrixes)
            {
                foreach (var matrix in pResMin)
                {
                    PrintMatrix(matrix);
                    Console.WriteLine();
                }
            }
            Console.WriteLine($"p max: {pValMax}");
            if (ApplicationConfiguration.OutputMatrixes)
            {
                foreach (var matrix in pResMax)
                {
                    PrintMatrix(matrix);
                    Console.WriteLine();
                }
            }
            Console.WriteLine($"p+ min: {pPlusValMin}");
            if (ApplicationConfiguration.OutputMatrixes)
            {
                foreach (var matrix in pPlusResMin)
                {
                    PrintMatrix(matrix);
                    Console.WriteLine();
                }
            }
            Console.WriteLine($"p+ max: {pPlusValMax}");
            if (ApplicationConfiguration.OutputMatrixes)
            {
                foreach (var matrix in pPlusResMax)
                {
                    PrintMatrix(matrix);
                    Console.WriteLine();
                }
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