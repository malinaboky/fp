using System.Drawing;

namespace TagsCloudVisualization.Distributors;

public interface ICloudDistribution
{
    public Point GetNextPoint();
}