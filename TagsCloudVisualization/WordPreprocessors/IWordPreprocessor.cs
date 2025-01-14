using TagsCloudVisualization.FuncMonad;

namespace TagsCloudVisualization.WordPreprocessors;

public interface IWordPreprocessor
{
    public Result<IEnumerable<Tuple<string, int>>> ProcessTextToWords(string text);
}