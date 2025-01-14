using System.Drawing;
using TagsCloudVisualization.ConsoleCommands;

namespace TagsCloudVisualization.Renderers.ColorGenerators;

public class GradientColorGenerator : IColorGenerator
{
    private readonly Color startColor = Color.FromArgb(69, 10, 92);
    private readonly Color middleColor = Color.FromArgb(31, 158, 136);
    private readonly Color endColor = Color.FromArgb(243, 229, 37);
    private readonly int numOfColors;
    private float stepCount;
    
    public GradientColorGenerator(Options options)
        => numOfColors = options.NumOfColors;
    
    public Color GetColor()
    {
        var color = endColor;
        
        if (stepCount >= numOfColors)
            return color;
        
        var halfCount = numOfColors / 2;

        color = stepCount < halfCount 
            ? GetNextColor(startColor, middleColor, stepCount / (halfCount - 1)) 
            : GetNextColor(middleColor, endColor, (stepCount - halfCount) / (numOfColors - halfCount - 1));
        
        stepCount++;
        
        return color;
    }

    private static Color GetNextColor(Color start, Color end, float ratio)
    {
        var red = Interpolate(start.R, end.R, ratio);
        var green = Interpolate(start.G, end.G, ratio);
        var blue = Interpolate(start.B, end.B, ratio);
        
        return Color.FromArgb(red, green, blue);
    }
    
    private static int Interpolate(int start, int end, float ratio)
    {
        return (int)(start + (end - start) * ratio);
    }
}