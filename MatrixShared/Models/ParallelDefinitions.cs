namespace MatrixShared.Models
{
    public static class ParallelDefinitions
    {
        public static readonly ParallelOptions ParallelOptions = new() { MaxDegreeOfParallelism = Environment.ProcessorCount };
        public static readonly int ExpectedParallelsCount = Environment.ProcessorCount;
    }
}
