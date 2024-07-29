using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Drawing.Processing;

namespace MatrixAlg.Helpers;

/// <summary>
/// Helper class, used to draw mosaics
/// </summary>
internal static class MosaicDrawer
{
    private const string OutputPathName = "mosaics";
    private const int CellSize = 20;

    public static void Clear()
    {
        if (Directory.Exists(OutputPathName))
        {
            Directory.Delete(OutputPathName, true);
        }
        Directory.CreateDirectory(OutputPathName);
    }

    public static void Draw(bool[,] mosaic, string name)
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
                Thread.Sleep(100);
            }
        }
    }
}
