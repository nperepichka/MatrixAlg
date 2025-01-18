namespace MatrixShared.Helpers;

public static class PFunctionsHelper
{
    public static List<byte[]> CalculatePartitions(byte size, bool outputResult = false)
    {
        var result = new List<byte[]>();
        var currentPartition = new List<byte>();

        void GeneratePartitions(byte remaining)
        {
            currentPartition.Add(0);
            var lastIndex = currentPartition.Count - 1;

            for (byte i = 1; i <= remaining; i++)
            {
                currentPartition[lastIndex] = i;

                if (remaining == i)
                {
                    result.Add([.. currentPartition]);
                }
                else
                {
                    GeneratePartitions((byte)(remaining - i));
                }
            }

            currentPartition.RemoveAt(lastIndex);
        }

        GeneratePartitions(size);

        if (outputResult)
        {
            Console.WriteLine("Partitions:");
            foreach (var partition in result)
            {
                Console.WriteLine(string.Join('+', partition));
            }
        }

        return result;
    }

    public static (int pFunc, int pPlusFunc) CalculatePFunctions(this int[,] matrix, List<byte[]> partitions, byte size)
    {
        var pFunc = 0;
        var pPlusFunc = 0;

        foreach (var partition in partitions)
        {
            var part = matrix.GetPartitionNumber(partition);
            if (part != 0)
            {
                if ((size - partition.Length) % 2 == 0)
                {
                    pFunc += part;
                }
                else
                {
                    pFunc -= part;
                }
                pPlusFunc += part;
            }
        }

        return (pFunc, pPlusFunc);
    }

    private static int GetPartitionNumber(this int[,] matrix, byte[] partition)
    {
        var res = 1;
        var x = 0;
        var y = -1;

        foreach (var p in partition)
        {
            var oldY = y;
            y += p;
            for (var i = y; i > oldY; i--)
            {
                var e = matrix[x, i];
                if (e == 0)
                {
                    return 0;
                }
                res *= e;
                x++;
            }
        }

        return res;
    }
}
