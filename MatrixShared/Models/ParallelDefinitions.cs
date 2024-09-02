namespace MatrixShared.Models
{
    public static class ParallelDefinitions
    {
        public static readonly ParallelOptions ParallelOptions = new() { MaxDegreeOfParallelism = -1 };
        public static readonly int ExpectedParallelsCount = Environment.ProcessorCount / 2;
    }
}
