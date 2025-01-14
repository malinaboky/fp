using FluentAssertions;
using TagsCloudVisualization.MyStemWrapper;
using TagsCloudVisualization.WordPreprocessors.WordValidators;

namespace TagsCloudVisualizationTests;

[TestFixture]
public class WordValidatorTests
{
    [TestCaseSource(nameof(WordValidatorSourceTestCases))]
    public void IsValid_ShouldValidateCorrectly(WordInfo wordInfo, bool isValid)
    {
        new DefaultWordValidator().IsValid(wordInfo).Should().Be(isValid);
    }
    
    private static IEnumerable<TestCaseData> WordValidatorSourceTestCases()
    {
        yield return new TestCaseData(new WordInfo { Grammeme = "CONJ", Lemma = "text" }, false);
        yield return new TestCaseData(new WordInfo { Grammeme = "INTJ", Lemma = "text" }, false);
        yield return new TestCaseData(new WordInfo { Grammeme = "PART", Lemma = "text" }, false);
        yield return new TestCaseData(new WordInfo { Grammeme = "PR", Lemma = "text" }, false);
        yield return new TestCaseData(new WordInfo { Grammeme = "SPRO", Lemma = "text" }, false);
        yield return new TestCaseData(new WordInfo { Grammeme = "V", Lemma = "text" }, true);
        yield return new TestCaseData(new WordInfo { Grammeme = "NUM", Lemma = "text" }, true);
        yield return new TestCaseData(new WordInfo { Grammeme = "A", Lemma = "text" }, true);
    }
}