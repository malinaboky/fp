using System.Drawing;

namespace TagsCloudVisualization.Renderers.ColorGenerators;

public interface IColorGenerator
{
    public Color GetColor();
}