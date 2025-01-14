using TagsCloudVisualization.Domain;
using TagsCloudVisualization.FuncMonad;

namespace TagsCloudVisualization.Renderers;

public interface ICloudRenderer
{
    public Result<None> Render(IEnumerable<Tag> tags);
}