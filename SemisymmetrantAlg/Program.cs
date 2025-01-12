using SemisymmetrantAlg.Helpers;

namespace SemisymmetrantAlg;

internal class Program
{
    private static byte Size = 0;

    private static int[,] Matrix = new int[0, 0];

    private static void Main()
    {
        Matrix = InputReader.ReadMatrix();
        Size = (byte)Matrix.GetLength(0);
        Console.WriteLine($"Size: {Size}");

        var value = 0;
        Console.WriteLine();
        Console.WriteLine("Partitions:");

        var partitions = PartitionsHelper.CalculatePartitions(Size);
        foreach (var partition in partitions)
        {
            Console.WriteLine(string.Join('+', partition));
            var n = GetPartitionNumber(partition);
            var coef = (Size - partition.Length) % 2 == 0 ? 1 : -1;
            //Console.WriteLine(coef * n);
            value += coef * n;
        }

        Console.WriteLine();
        Console.WriteLine($"Semisymmetrant: {value}");
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
                //Console.WriteLine(Matrix[x, i]);
            }
        }

        return res;
    }


}