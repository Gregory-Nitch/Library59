using SkiaSharp;

namespace Library59.ImageProcessing;

/// <summary>
/// Merges Jpegs to single Png.
/// </summary>
public class JpegMerger
{
    /// <summary>
    /// Merges a 2d list of image paths to a single SKBitmap.
    /// </summary>
    /// <param name="imgPathMatrix">2d list of image filenames</param>
    /// <param name="imgsDir">directory containing images to merge</param>
    /// <param name="imgHeights">height of images (assumed same size)</param>
    /// <param name="imgWidths">width of images (assumed same size)</param>
    /// <returns>SKBitmap of the merged images</returns>
    public static SKBitmap MergeFrom2DList(List<List<string>> imgPathMatrix,
                                           string imgsDir,
                                           int imgHeights,
                                           int imgWidths)
    {
        List<List<SKImage>> imgsToMerge = [];
        int outHeight = imgPathMatrix.Count * imgHeights;
        int outWidth = imgPathMatrix.First().Count * imgWidths;

        // Load images
        foreach (List<string> row in imgPathMatrix)
        {
            List<SKImage> imgRow = [];
            foreach (string imgPath in row)
            {
                string fullpath = Path.Combine(imgsDir, imgPath);
                using (FileStream fs = File.OpenRead(fullpath))
                {
                    var img = SKImage.FromEncodedData(fs);
                    imgRow.Add(img);
                }
            }
            imgsToMerge.Add(imgRow);
        }

        // Create canvas, draw to it, pull merged Bitmap from canvas
        var surfaceInfo = new SKImageInfo(outWidth, outHeight);
        SKSurface surface = SKSurface.Create(surfaceInfo);
        var canvas = surface.Canvas;

        int x;
        int y = 0;
        foreach (List<SKImage> imgRow in imgsToMerge)
        {
            x = 0;
            foreach (SKImage img in imgRow)
            {
                canvas.DrawImage(img, x, y, paint: null);
                img.Dispose();
                x += imgWidths;
            }
            y += imgHeights;
        }

        var outMap = SKBitmap.FromImage(surface.Snapshot());
        surface.Dispose();
        return outMap;
    }

    /// <summary>
    /// Merges Jpegs with call to MergeFrom2DList() and saves to Png.
    /// </summary>
    /// <param name="imgPathMatrix">2d list of image filenames</param>
    /// <param name="imgsDir">directory with images to merge</param>
    /// <param name="imgHeights">height of images (assumed same size)</param>
    /// <param name="imgWidths">width of images (assumed same size)</param>
    /// <param name="outPath">combined directory and filename for output image</param>
    public static void MergeFrom2DListAndSaveToPng(List<List<string>> imgPathMatrix,
                                       string imgsDir,
                                       int imgHeights,
                                       int imgWidths,
                                       string outPath)
    {
        SKBitmap outMap = MergeFrom2DList(imgPathMatrix, imgsDir, imgHeights, imgWidths);

        using (var fs = new FileStream(outPath, FileMode.Create))
        {
            outMap.Encode(fs, SKEncodedImageFormat.Png, 100);
            outMap.Dispose();
        }
    }
}
