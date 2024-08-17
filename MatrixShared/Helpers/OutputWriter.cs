using MatrixShared.Models;
using System.Collections.Concurrent;
using System.Text;

namespace MatrixShared.Helpers;

/// <summary>
/// Helper class, used to manage output
/// </summary>
public static class OutputWriter
{
    /// <summary>
    /// Output file name
    /// </summary>
    private const string OutputFileName = "output.txt";
    /// <summary>
    /// Queue of strings for output
    /// </summary>
    private static readonly ConcurrentQueue<string> OutputQueue = new();
    /// <summary>
    /// Output queue monitoring enabled flag
    /// </summary>
    private static bool OutputQueueMonitoringEnabled { get; set; } = false;

    /// <summary>
    /// Clear output file
    /// </summary>
    public static void Clear()
    {
        // Check if output file exists
        if (File.Exists(OutputFileName))
        {
            // Remove output file
            File.Delete(OutputFileName);
        }
    }

    /// <summary>
    /// Add string value to output queue
    /// </summary>
    /// <param name="s">Value</param>
    public static void Write(string s)
    {
        // Add string value to output queue
        OutputQueue.Enqueue(s);
    }

    /// <summary>
    /// Add string value to output queue with new line symbol
    /// </summary>
    /// <param name="s">Value</param>
    public static void WriteLine(string? s = null)
    {
        // Add string value to output queue with new line symbol
        Write($"{s}{Environment.NewLine}");
    }

    public static void StartOutputQueueMonitoring()
    {
        OutputQueueMonitoringEnabled = true;
        Task.Run(() =>
        {
            var outputStringBuilder = new StringBuilder(string.Empty);

            while (OutputQueueMonitoringEnabled)
            {
                var outputBuilderCounter = 0;

                while (OutputQueue.TryDequeue(out var s))
                {
                    if (ApplicationConfiguration.EnableConsoleOutput)
                    {
                        Console.Write(s);
                    }
                    outputStringBuilder.Append(s);
                    outputBuilderCounter++;

                    if (outputBuilderCounter == 100)
                    {
                        break;
                    }
                }

                WriteToFile(ref outputStringBuilder, true);

                if (outputBuilderCounter == 0)
                {
                    Thread.Sleep(10);
                }
            }

            WriteToFile(ref outputStringBuilder, false);
        });
    }

    private static void WriteToFile(ref StringBuilder stringBuilder, bool allowIOException)
    {
        try
        {
            File.AppendAllText(OutputFileName, stringBuilder.ToString());
            stringBuilder.Clear();
        }
        catch (Exception ex)
        {
            if (!allowIOException || ex is not IOException)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
            }
        }
    }

    public static void StopOutputQueueMonitoring()
    {
        while (!OutputQueue.IsEmpty)
        {
            Thread.Sleep(10);
        }
        OutputQueueMonitoringEnabled = false;
    }
}
