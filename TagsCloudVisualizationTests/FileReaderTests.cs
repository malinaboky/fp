using Autofac;
using FluentAssertions;
using TagsCloudVisualization;
using TagsCloudVisualization.ConsoleCommands;
using TagsCloudVisualization.FileReaders;

namespace TagsCloudVisualizationTests;

[TestFixture]
public class FileReaderTests
{
    private ILifetimeScope Scope { get; set; }

    [SetUp]
    public void Setup()
    {
        var options = new Options
        {
            InputFilePath = "/path/in/test/not/needed",
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
    
    [TestCase("source\\input.txt", "Собака\r\nСобака\r\nСобака\r\nКошку\r\nКошки\r\nОн\r\nВ\r\nТвое")]
    [TestCase("source\\input.doc", "Собака\vСобака\vСобака\vКошку\vКошки\vОн\vВ\vТвое")]
    [TestCase("source\\input.docx", "Собака\nСобака\nСобака\nКошку\nКошки\nОн\nВ\nТвое")]
    public void Read_ShouldReturnCorrectStringFromFile(string path, string expected)
    {
        var absolutePath = Path.GetFullPath(path);
        var fileReaderFactory = Scope.Resolve<FileReaderFactory>();

        var result = fileReaderFactory.GetFileReader(absolutePath).GetValueOrThrow().Read(absolutePath);

        result.GetValueOrThrow().Should().BeEquivalentTo(expected);
    }
}