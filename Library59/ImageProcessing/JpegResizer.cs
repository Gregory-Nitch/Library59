using SkiaSharp;

namespace Library59.ImageProcessing;

/// <summary>
/// Resizes Jpegs based on input scales.
/// </summary>
public class JpegResizer
{
    /// <summary>
    /// Resizes Jpeg images based on input scale.
    /// </summary>
    /// <param name="inPath">path to image to resize</param>
    /// <param name="scale">requested output scale</param>
    /// <returns>SKBitmap of the resized image</returns>
    public static SKBitmap ResizeImg(string inPath,
                                     double scale)
    {
        SKBitmap originalMap;
        using (FileStream fs = File.OpenRead(inPath))
        {
            originalMap = SKBitmap.Decode(fs);
        }

        int outWidth = (int)(originalMap.Width * scale);
        int outHeight = (int)(originalMap.Height * scale);

        return originalMap.Resize(new SKImageInfo(outWidth,
                                                  outHeight),
                                                  SKFilterQuality.High);
    }

    /// <summary>
    /// Resizes Jpegs with call to ResizeImg() and then saves resulting SKBitmap to output file.
    /// </summary>
    /// <param name="inPath">path to image to resize</param>
    /// <param name="outPath">path to save resized image to</param>
    /// <param name="scale">requested output scale</param>
    public static void ResizeImgAndSaveToJpeg(string inPath,
                                              string outPath,
                                              double scale)
    {
        SKBitmap outMap = ResizeImg(inPath, scale);

        using (var fs = new FileStream(outPath, FileMode.Create))
        {
            outMap.Encode(fs, SKEncodedImageFormat.Jpeg, 100);
            outMap.Dispose();
        }
    }
}
