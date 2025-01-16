using PFunctionAlg.Helpers;

namespace PFunctionAlg;

internal class Program
{
    private static int[,] Matrix = new int[0, 0];

    private static void Main()
    {
        Matrix = InputReader.ReadMatrix();
        var size = (byte)Matrix.GetLength(0);
        Console.WriteLine($"Size: {size}");

        var pFunc = 0;
        var pFuncPlus = 0;
        Console.WriteLine();
        Console.WriteLine("Partitions:");

        var partitions = PartitionsHelper.CalculatePartitions(size);
        foreach (var partition in partitions)
        {
            Console.WriteLine(string.Join('+', partition));
            var part = GetPartitionNumber(partition);
            var coef = (size - partition.Length) % 2 == 0 ? 1 : -1;
            pFunc += coef * part;
            pFuncPlus += part;
        }

        Console.WriteLine();
        Console.WriteLine($"p  = {pFunc}");
        Console.WriteLine($"p+ = {pFuncPlus}");
    }

    private static int GetPartitionNumber(byte[] partition)
    {
        var res = 1;
        var x = -1;
        var y = -1;

        foreach (var p in partition)
        {
            var oldY = y;
            y += p;
            for (var i = y; i > oldY; i--)
            {
                x++;
                res *= Matrix[x, i];
            }
        }

        return res;
    }
}