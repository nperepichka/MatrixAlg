using MatrixShared.Models;

namespace MatrixShared.Helpers;

public static class ParallelsHelper
{
    private static readonly int MaxParallels = ParallelsConfiguration.MaxParallels;
    private static int ParallelsCount = 0;

    public static void RunInParallel(byte size, Action<byte> action)
    {
        if (MaxParallels <= ParallelsCount)
        {
            for (byte i = 0; i < size; i++)
            {
                action(i);
            }
        }
        else if (ParallelsCount != 0)
        {
            Interlocked.Increment(ref ParallelsCount);

            Parallel.For(0, size, new() { MaxDegreeOfParallelism = 2 }, i =>
            {
                action((byte)i);
            });

            Interlocked.Decrement(ref ParallelsCount);
        }
        else
        {
            Interlocked.Add(ref ParallelsCount, size);

            Parallel.For(0, size, new() { MaxDegreeOfParallelism = size }, i =>
            {
                action((byte)i);
                Interlocked.Decrement(ref ParallelsCount);
            });
        }
    }
}
