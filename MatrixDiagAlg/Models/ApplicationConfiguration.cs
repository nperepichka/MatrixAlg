using MatrixShared.Helpers;

namespace MatrixDiagAlg.Models;

public static class ApplicationConfiguration
{
    public static int MaxParallelization { get; set; } = ConfigurationHelper.GetValue(nameof(MaxParallelization), Environment.ProcessorCount);
}
