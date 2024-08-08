namespace MatrixAlg.Models;

internal static class ApplicationConfiguration
{
    public static bool AnalyzeCubes { get; private set; } = false;
    public static bool OutputCubeDetails { get; private set; } = false;
    public static bool DrawMosaics { get; private set; } = false;
    public static bool OutputDecompositions { get; private set; } = true;

    public static void Init()
    {
        try
        {
            var values = File
                .ReadAllLines("config.ini")
                .Select(l => l.Split(";")[0].Trim())
                .Where(l => !string.IsNullOrEmpty(l))
                .Select(l => l.Split("="))
                .ToDictionary(k => k[0].Trim(), v => int.TryParse(v[1].Trim(), out var val) ? val : 0);

            AnalyzeCubes = values.GetConfigFlag(nameof(AnalyzeCubes), AnalyzeCubes);
            OutputCubeDetails = values.GetConfigFlag(nameof(OutputCubeDetails), OutputCubeDetails);
            DrawMosaics = values.GetConfigFlag(nameof(DrawMosaics), DrawMosaics);
            OutputDecompositions = values.GetConfigFlag(nameof(OutputDecompositions), OutputDecompositions);
        }
        catch (Exception ex)
        {
            throw new Exception($"Invalid configuration file ({ex.Message}).");
        }
    }

    private static bool GetConfigFlag(this Dictionary<string, int> values, string valueName, bool dafalutValue)
    {
        return GetConfigFlag(values, valueName, dafalutValue, out _);
    }

    private static bool GetConfigFlag(this Dictionary<string, int> values, string valueName, bool dafalutValue, out int configValue)
    {
        try
        {
            if (values.TryGetValue(valueName, out var value))
            {
                configValue = value;
                return value != 0;
            }
            configValue = 0;
            return false;
        }
        catch
        {
            configValue = 0;
            return dafalutValue;
        }
    }
}
