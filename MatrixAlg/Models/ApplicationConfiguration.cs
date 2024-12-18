﻿namespace MatrixAlg.Models;

public static class ApplicationConfiguration
{
    public static bool OutputDecompositions { get; private set; } = true;
    public static bool EnableConsoleOutput { get; private set; } = true;

    static ApplicationConfiguration()
    {
        Init();
    }

    private static void Init()
    {
        try
        {
            var values = File
                .ReadAllLines("config.ini")
                .Select(l => l.Split(";")[0].Trim())
                .Where(l => !string.IsNullOrEmpty(l))
                .Select(l => l.Split("="))
                .ToDictionary(k => k[0].Trim(), v => int.TryParse(v[1].Trim(), out var val) ? val : 0);

            OutputDecompositions = values.GetConfigFlag(nameof(OutputDecompositions), OutputDecompositions);
            EnableConsoleOutput = values.GetConfigFlag(nameof(EnableConsoleOutput), EnableConsoleOutput);
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
