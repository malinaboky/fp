using System.Drawing;
using TagsCloudVisualization.ConsoleCommands;
using TagsCloudVisualization.Enums;

namespace TagsCloudVisualization.BitmapProcessors;

public class DefaultBitmapProcessor : IBitmapProcessor
{
    private readonly OutputImageFormat imageFormat;
    
    public DefaultBitmapProcessor(Options options)
        => imageFormat = options.ImageFormat;
    
    public void SaveImage(Bitmap bitmap, string imageDirectory, string imageName)
    {
        var filePath = Path.Combine(imageDirectory, $"{imageName}.{imageFormat.ToString().ToLowerInvariant()}");
        bitmap.Save(filePath);
    }
}