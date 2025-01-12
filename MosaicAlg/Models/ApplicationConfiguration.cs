using MatrixShared.Helpers;

namespace MosaicAlg.Models;

public static class ApplicationConfiguration
{
    public static readonly int MaxParallelization = ConfigurationHelper.GetValue(nameof(MaxParallelization), Environment.ProcessorCount);
}
