using SkiaSharp;

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

        using var bitmap = new SKBitmap(size * CellSize, size * CellSize);
        using var canvas = new SKCanvas(bitmap);
        for (var row = 0; row < size; row++)
        {
            for (var col = 0; col < size; col++)
            {
                // Determine the color of the current cell
                var cellColor = mosaic[row, col] ? SKColors.Black : SKColors.White;

                // Define the rectangle for the current cell
                var rect = new SKRect(col * CellSize, row * CellSize, (col + 1) * CellSize, (row + 1) * CellSize);

                // Fill the rectangle with the cell color
                using var paint = new SKPaint { Color = cellColor };
                canvas.DrawRect(rect, paint);
            }
        }

        // Save the bitmap as a PNG file
        using var image = SKImage.FromBitmap(bitmap);
        using var data = image.Encode(SKEncodedImageFormat.Png, 100);
        using var stream = File.OpenWrite($"mosaics/{name}.png");
        data.SaveTo(stream);
    }
}
