using System.Drawing;

namespace TagsCloudVisualization.BitmapProcessors;

public interface IBitmapProcessor
{
    public void SaveImage(Bitmap bitmap, string imageDirectory, string imageName);
}