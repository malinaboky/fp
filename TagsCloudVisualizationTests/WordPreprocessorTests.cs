using Autofac;
using FluentAssertions;
using TagsCloudVisualization;
using TagsCloudVisualization.ConsoleCommands;
using TagsCloudVisualization.FileReaders;
using TagsCloudVisualization.FuncMonad;
using TagsCloudVisualization.WordPreprocessors;

namespace TagsCloudVisualizationTests;

[TestFixture]
public class WordPreprocessorTests
{
    private static readonly string PathToTestFile = Path.GetFullPath("source\\input.txt");
    private ILifetimeScope Scope { get; set; }

    [SetUp]
    public void Setup()
    {
        var options = new Options
        {
            InputFilePath = PathToTestFile,
            OutputDirectory = "/path/in/test/not/needed",
        };
        var container = ContainerConfig.Configure(options);
        Scope = container.BeginLifetimeScope();
    }

    [TearDown]
    public void TearDown()
    {
        Scope.Dispose();
    }

    [Test]
    public void ProcessTextToWords_ShouldContainOnlyNotBoringWordsReducedToInitialFormWithLowerCase()
    {
        var words = ReadTestFile();
        var result = new List<Tuple<string, int>>
        {
            new("кошка", 2),
            new("собака", 3)
        };

        var wordProcessor = Scope.Resolve<IWordPreprocessor>();
        wordProcessor.ProcessTextToWords(words).GetValueOrThrow().Should().BeEquivalentTo(result);

    }

    [Test]
    public void ProcessTextToWords_ShouldBeOrderedByNumberOfWords()
    {
        var words = ReadTestFile();
        
        var wordProcessor = Scope.Resolve<IWordPreprocessor>();
        wordProcessor.ProcessTextToWords(words).GetValueOrThrow().Should().BeInDescendingOrder(word => word.Item2);
    }

    private string ReadTestFile() =>
        Scope.Resolve<FileReaderFactory>()
            .GetFileReader(PathToTestFile)
            .Then(reader => reader.Read(PathToTestFile))
            .GetValueOrThrow();
}