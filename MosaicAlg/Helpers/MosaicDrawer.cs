using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace MosaicAlg.Helpers;

/// <summary>
/// Helper class, used to draw mosaics
/// </summary>
internal static class MosaicDrawer
{
    private const string OutputPathName = "mosaics";
    private const int CellSize = 10;

    private static readonly object MosaicLock = new();
    private static readonly HashSet<string> MosaicHashes = [];

    public static void Clear()
    {
        if (Directory.Exists(OutputPathName))
        {
            Directory.Delete(OutputPathName, true);
        }
        Directory.CreateDirectory(OutputPathName);
    }

    public static void Draw(byte[][] decomposition, byte size)
    {
        var mosaic = new bool[size, size];
        for (var i = 0; i < decomposition.Length; i++)
        {
            for (var j = 0; j < decomposition[i].Length; j++)
            {
                mosaic[j, decomposition[i][j]] = true;
            }
        }

        var hash = GetHash(mosaic);
        var isNew = false;
        var n = 0;
        lock (MosaicLock)
        {
            if (MosaicHashes.Add(hash))
            {
                n = MosaicHashes.Count;
                isNew = true;
            }
        }
        if (isNew)
        {
            DrawImage(mosaic, $"mosaic_{n}");
        }
    }

    private static string GetHash(bool[,] mosaic)
    {
        var size = mosaic.GetLength(0);
        var flatArray = new char[(size * size + 15) / 16];

        var bitIndex = 0;
        for (var i = 0; i < size; i++)
        {
            for (var j = 0; j < size; j++)
            {
                if (mosaic[i, j])
                {
                    flatArray[bitIndex / 16] |= (char)(1 << (bitIndex % 16));
                }
                bitIndex++;
            }
        }

        return new string(flatArray);
    }

    private static void DrawImage(bool[,] mosaic, string name)
    {
        var size = mosaic.GetLength(0);

        using var image = new Image<Rgba32>(size * CellSize, size * CellSize);
        for (var row = 0; row < size; row++)
        {
            for (var col = 0; col < size; col++)
            {
                // Determine the color of the current cell
                var cellColor = mosaic[row, col] ? Color.Black : Color.White;

                // Define the rectangle for the current cell
                var rect = new Rectangle(col * CellSize, row * CellSize, CellSize, CellSize);

                // Fill the rectangle with the cell color
                image.Mutate(ctx => ctx.Fill(cellColor, rect));
            }
        }

        var attempt = 1;
        while (true)
        {
            try
            {
                // Save the image as a PNG file
                image.Save($"mosaics/{name}.png");
                break;
            }
            catch (Exception ex)
            {
                if (attempt == 5 || ex is not IOException)
                {
                    Console.WriteLine($"Error occurred: {ex.Message}");
                    break;
                }
                attempt++;
                Thread.Sleep(10);
            }
        }
    }
}
