using TagsCloudVisualization.MyStemWrapper;

namespace TagsCloudVisualization.WordPreprocessors.WordValidators;

public interface IWordValidator
{
    public bool IsValid(WordInfo wordInfo);
}