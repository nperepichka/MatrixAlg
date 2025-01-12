using MatrixShared.Helpers;

namespace CubeAlg.Models;

public static class ApplicationConfiguration
{
    public static readonly bool OutputAsCoordinates = ConfigurationHelper.GetFlag(nameof(OutputAsCoordinates), false);
}
