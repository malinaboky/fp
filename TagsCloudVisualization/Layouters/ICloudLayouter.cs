using TagsCloudVisualization.Domain;
using TagsCloudVisualization.FuncMonad;

namespace TagsCloudVisualization.Layouters;

public interface ICloudLayouter
{
    public Result<IEnumerable<Tag>> CreateTagsCloud(IEnumerable<Tuple<string, int>> wordsCollection);
}