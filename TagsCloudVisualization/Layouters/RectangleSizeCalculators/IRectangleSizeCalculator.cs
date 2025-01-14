using System.Drawing;

namespace TagsCloudVisualization.Layouters.RectangleSizeCalculators;

public interface IRectangleSizeCalculator
{
    public Size ConvertWordToRectangleSize(string word, Font font);
}