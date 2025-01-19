using System.Drawing;
using TagsCloudVisualization.FuncMonad;

namespace TagsCloudVisualization.WordPreprocessors.FontCreators;

public interface IFontCreator
{
    public Result<Font> CreateFont(int fontSizeFactor, int minWordCount, int maxWordCount);
}