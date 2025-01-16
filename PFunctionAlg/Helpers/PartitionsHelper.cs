namespace PFunctionAlg.Helpers;

internal class PartitionsHelper
{
    internal static List<byte[]> CalculatePartitions(byte size)
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
        return result;
    }
}
