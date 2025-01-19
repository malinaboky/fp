using System.Drawing;
using TagsCloudVisualization.FuncMonad;

namespace TagsCloudVisualization.Layouters.RectangleSizeCalculators;

public interface IRectangleSizeCalculator
{
    public Result<Size> ConvertWordToRectangleSize(string word, Font font);
}