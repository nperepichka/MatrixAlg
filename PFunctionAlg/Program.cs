using MatrixShared.Helpers;
using PFunctionAlg.Helpers;

namespace PFunctionAlg;

internal class Program
{
    private static void Main()
    {
        var matrix = InputReader.ReadMatrix();
        var size = (byte)matrix.GetLength(0);
        Console.WriteLine($"Size: {size}");
        Console.WriteLine();

        var partitions = PFunctionsHelper.CalculatePartitions(size, true);
        var (pFunc, pPlusFunc) = PFunctionsHelper.CalculatePFunctions(matrix, partitions);

        Console.WriteLine();
        Console.WriteLine($"p : {pFunc}");
        Console.WriteLine($"p+: {pPlusFunc}");
    }


}