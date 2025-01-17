using MatrixShared.Helpers;

namespace PFunctionAnalyzerAlg.Models;

public static class ApplicationConfiguration
{
    public static readonly bool OutputMatrixes = ConfigurationHelper.GetFlag(nameof(OutputMatrixes), false);
}
