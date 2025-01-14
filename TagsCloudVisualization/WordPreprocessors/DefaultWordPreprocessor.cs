using System.Text.Json;
using TagsCloudVisualization.FuncMonad;
using TagsCloudVisualization.MyStemWrapper;
using TagsCloudVisualization.WordPreprocessors.WordValidators;

namespace TagsCloudVisualization.WordPreprocessors;

public class DefaultWordPreprocessor : IWordPreprocessor
{
    private IWordValidator wordValidator;
    private MyStem myStem;

    public DefaultWordPreprocessor(IWordValidator wordValidator, MyStem myStem)
    {
        this.wordValidator = wordValidator;
        this.myStem = myStem;
    }
    
    public Result<IEnumerable<Tuple<string, int>>> ProcessTextToWords(string text)
    {
        return GetMorphologicalAnalysis(text)
            .Then(GetStatisticsOnWords)
            .Then(statistic =>
                statistic.Select(x => new Tuple<string, int>(x.Key, x.Value))
                    .OrderByDescending(x => x.Item2)
                    .AsEnumerable());
    }
    
    private Result<string[]> GetMorphologicalAnalysis(string text) 
        => Result.Of(() => myStem.Analysis(text).Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
            .RefineError("Error connecting to MyStem");

    private Dictionary<string, int> GetStatisticsOnWords(string[] analysis)
    {
        var result = new Dictionary<string, int>();
        
        foreach (var wordInfo in analysis)
        {
            var dto = JsonSerializer.Deserialize<MyStemDto>(wordInfo);
            var word = dto.Analysis.First();
                
            if (!wordValidator.IsValid(word)) continue;
                
            if (!result.TryAdd(word.Lemma, 1))
                result[word.Lemma] += 1;
        }
        
        return result;
    }
}