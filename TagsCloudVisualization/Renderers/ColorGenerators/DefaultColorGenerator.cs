using System.Drawing;

namespace TagsCloudVisualization.Renderers.ColorGenerators;

public class DefaultColorGenerator : IColorGenerator
{
    private readonly Random random = new();
    
    public Color GetColor()
    {
        return Color.FromArgb(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));
    }
}