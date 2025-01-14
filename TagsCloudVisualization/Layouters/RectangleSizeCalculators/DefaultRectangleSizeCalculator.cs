using System.Drawing;

namespace TagsCloudVisualization.Layouters.RectangleSizeCalculators;

public class DefaultRectangleSizeCalculator : IRectangleSizeCalculator
{
    public Size ConvertWordToRectangleSize(string word, Font font)
    {
        using var graphic = Graphics.FromImage(new Bitmap(1, 1));
        var sizeF = graphic.MeasureString(word, font);
        return new Size((int)Math.Ceiling(sizeF.Width), (int)Math.Ceiling(sizeF.Height));
    }
}