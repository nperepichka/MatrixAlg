namespace MatrixShared.Models;

public static class ParallelsConfiguration
{
    public static int MaxParallels { get; private set; } = Environment.ProcessorCount;

    static ParallelsConfiguration()
    {
        Init();
    }

    private static void Init()
    {
        try
        {
            var values = File
                .ReadAllLines("parallels_config.ini")
                .Select(l => l.Split(";")[0].Trim())
                .Where(l => !string.IsNullOrEmpty(l))
                .Select(l => l.Split("="))
                .ToDictionary(k => k[0].Trim(), v => int.TryParse(v[1].Trim(), out var val) ? val : 0);

            MaxParallels = values.GetConfigFlag(nameof(MaxParallels), MaxParallels);
        }
        catch (Exception ex)
        {
            throw new Exception($"Invalid parallels configuration file ({ex.Message}).");
        }
    }

    private static int GetConfigFlag(this Dictionary<string, int> values, string valueName, int dafalutValue)
    {
        return values.TryGetValue(valueName, out var value)
            ? value
            : 0;
    }
}
