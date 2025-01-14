using System.Drawing;

namespace TagsCloudVisualization.Domain;

public class Tag
{
    public readonly Rectangle Rectangle;
    public readonly Font Font;
    public readonly string Content;

    public Tag(Rectangle rectangle, Font font, string content)
    {
        Rectangle = rectangle;
        Font = font;
        Content = content;
    }
}