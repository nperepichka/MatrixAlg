namespace MatrixShared.Helpers;

public static class ConfigurationHelper
{
    private static Dictionary<string, int> ConfigValues { get; set; } = InitConfigValues();

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
        return GetFlag(valueName, dafalutValue, out _);
    }

    private static bool GetFlag(string valueName, bool dafalutValue, out int configValue)
    {
        try
        {
            if (ConfigValues.TryGetValue(valueName, out var value))
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
