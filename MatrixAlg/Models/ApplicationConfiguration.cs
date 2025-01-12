using MatrixShared.Helpers;

namespace MatrixAlg.Models;

public static class ApplicationConfiguration
{
    public static readonly bool OutputDecompositions = ConfigurationHelper.GetFlag(nameof(OutputDecompositions), true);
    public static readonly bool EnableConsoleOutput = ConfigurationHelper.GetFlag(nameof(EnableConsoleOutput), true);
    public static readonly int MaxParallelization = ConfigurationHelper.GetValue(nameof(MaxParallelization), Environment.ProcessorCount);
}
