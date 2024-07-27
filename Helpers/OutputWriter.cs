using System.Collections.Concurrent;
using System.Text;

namespace MatrixAlg.Helpers;

/// <summary>
/// Helper class, used to manage output
/// </summary>
internal static class OutputWriter
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
    /// Can write to console flag
    /// </summary>
    public static bool CanWriteToConsole { get; set; } = true;
    /// <summary>
    /// Queue of strings for output monitoring enabled flag
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

    public static void Write(string s)
    {
        OutputQueue.Enqueue(s);
    }

    public static void WriteLine(string? s = null)
    {
        Write($"{s}{Environment.NewLine}");
    }

    public static void StartOutputQueueMonitoring()
    {
        OutputQueueMonitoringEnabled = true;
        Task.Run(() =>
        {
            while (OutputQueueMonitoringEnabled)
            {
                var outputStringBuilder = new StringBuilder(string.Empty);
                var outputBuilderCounter = 0;

                while (OutputQueue.TryDequeue(out var s))
                {
                    if (CanWriteToConsole)
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

                File.AppendAllText(OutputFileName, outputStringBuilder.ToString());

                if (outputBuilderCounter == 0)
                {
                    Thread.Sleep(10);
                }
            }
        });
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
