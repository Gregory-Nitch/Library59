
using SkiaSharp;

namespace Library59.ImageProcessing;

public class JpegResizer
{
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
