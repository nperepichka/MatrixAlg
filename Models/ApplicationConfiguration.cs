namespace MatrixAlg.Models;

internal static class ApplicationConfiguration
{
    public static bool AnalyzeCube { get; private set; } = false;
    public static bool DrawMosaics { get; private set; } = false;

    public static void Init()
    {
        try
        {
            var values = File
                .ReadAllLines("config.ini")
                .Select(l => l.Split(";")[0].Trim())
                .Where(l => !string.IsNullOrEmpty(l))
                .Select(l => l.Split("="))
                .ToDictionary(k => k[0].Trim(), v => v[1].Trim());

            AnalyzeCube = values.GetConfigFlag(nameof(AnalyzeCube), AnalyzeCube);
            DrawMosaics = values.GetConfigFlag(nameof(DrawMosaics), DrawMosaics);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Invalid configuration file ({ex.Message}). Using default configuration.");
        }
    }

    private static bool GetConfigFlag(this Dictionary<string, string> values, string valueName, bool dafalutValue)
    {
        try
        {
            return values.TryGetValue(valueName, out var value) && value == "1";
        }
        catch
        {
            return dafalutValue;
        }
    }
}
