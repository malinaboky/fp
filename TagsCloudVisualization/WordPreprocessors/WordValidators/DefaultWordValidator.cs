using TagsCloudVisualization.MyStemWrapper;

namespace TagsCloudVisualization.WordPreprocessors.WordValidators;

public class DefaultWordValidator : IWordValidator
{
    public bool IsValid(WordInfo wordInfo)
    {
        return !(wordInfo.Grammeme.Contains("CONJ") || 
                 wordInfo.Grammeme.Contains("INTJ") || 
                 wordInfo.Grammeme.Contains("PART") || 
                 wordInfo.Grammeme.Contains("PR") || 
                 wordInfo.Grammeme.Contains("SPRO"));
    }
}