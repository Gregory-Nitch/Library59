
using SkiaSharp;

namespace Library59.ImageProcessing;

public class JpegMerger
{
    public static SKBitmap MergeFrom2DList(List<List<string>> imgPathMatrix,
                                           string imgsDir,
                                           int imgHeights,
                                           int imgWidths)
    {
        List<List<SKImage>> imgsToMerge = [];
        int outHeight = imgPathMatrix.Count * imgHeights;
        int outWidth = imgPathMatrix.First().Count * imgWidths;

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
