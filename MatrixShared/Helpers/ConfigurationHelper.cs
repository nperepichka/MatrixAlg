namespace MatrixShared.Helpers;

public static class ConfigurationHelper
{
    private static readonly Dictionary<string, int> ConfigValues = InitConfigValues();

    private static Dictionary<string, int> InitConfigValues()
    {
        try
        {
            return File
                .ReadAllLines("config.ini")
                .Select(l => l.Split(";")[0].Trim())
                .Where(l => !string.IsNullOrEmpty(l))
                .Select(l => l.Split("="))
                .ToDictionary(k => k[0].Trim(), v => int.TryParse(v[1].Trim(), out var val) ? val : 0);
        }
        catch (Exception ex)
        {
            throw new Exception($"Invalid configuration file ({ex.Message}).");
        }
    }

    public static bool GetFlag(string valueName, bool dafalutValue)
    {
        return ConfigValues.TryGetValue(valueName, out var value)
            ? value != 0
            : dafalutValue;
    }

    public static int GetValue(string valueName, int dafalutValue)
    {
        return ConfigValues.TryGetValue(valueName, out var value)
            ? value
            : dafalutValue;
    }
}
