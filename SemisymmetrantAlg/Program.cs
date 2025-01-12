using SemisymmetrantAlg.Helpers;

namespace SemisymmetrantAlg;

internal class Program
{
    private static byte Size = 0;

    private static void Main()
    {
        var matrix = InputReader.ReadMatrix();
        Size = (byte)matrix.GetLength(0);
        Console.WriteLine($"Size: {Size}");

        var partitions = PartitionsHelper.CalculatePartitions(5);
        foreach (var partition in partitions)
        {
            Console.WriteLine(string.Join("+", partition));
        }

        // TODO: implement
    }


}