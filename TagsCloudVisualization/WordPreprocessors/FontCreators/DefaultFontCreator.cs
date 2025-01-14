using System.Drawing;
using TagsCloudVisualization.ConsoleCommands;

namespace TagsCloudVisualization.WordPreprocessors.FontCreators;

public class DefaultFontCreator : IFontCreator
{
    private readonly string fontName;
    private readonly int maxFontSize;
    private readonly int minFontSize;

    public DefaultFontCreator(Options options)
    {
        fontName = options.TagsFont;
        maxFontSize = options.MaxTagsFontSize;
        minFontSize = options.MinTagsFontSize;
    }
    
    public Font CreateFont(int fontSizeFactor, int minWordCount, int maxWordCount)
    {
        return minWordCount == maxWordCount 
            ? new Font(fontName, Math.Min(Math.Max(minFontSize, fontSizeFactor), maxFontSize)) 
            : new Font(fontName, NormalizeFontSize(fontSizeFactor, minWordCount, maxWordCount));
    }

    private int NormalizeFontSize(int fontSizeFactor, int minWordCount, int maxWordCount)
    {
        return minFontSize + (fontSizeFactor - minWordCount) / (maxWordCount - minWordCount) * (maxFontSize - minFontSize);
    }
}