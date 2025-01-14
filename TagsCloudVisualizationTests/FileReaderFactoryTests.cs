using Autofac;
using FluentAssertions;
using TagsCloudVisualization;
using TagsCloudVisualization.ConsoleCommands;
using TagsCloudVisualization.FileReaders;
using TagsCloudVisualization.FuncMonad;

namespace TagsCloudVisualizationTests;

[TestFixture]
public class FileReaderFactoryTests
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

    [TestCase("source\\input.txt", typeof(TextFileReader))]
    [TestCase("source\\input.doc", typeof(DocFileReader))]
    [TestCase("source\\input.docx", typeof(DocxFileReader))]
    public void GetFileReader_ShouldReturnCorrectFileReader(string path, Type expectedType)
    {
        var absolutePath = Path.GetFullPath(path);
        var fileReaderFactory = Scope.Resolve<FileReaderFactory>();

        fileReaderFactory.GetFileReader(absolutePath).GetValueOrThrow().Should().BeOfType(expectedType);
    }

    [Test]
    public void GetFileReader_ShouldThrowFileNotFoundException_WhenFileDoesNotExist()
    {
        var fileReaderFactory = Scope.Resolve<FileReaderFactory>();
        var action = fileReaderFactory.GetFileReader("any/path");
        
        action.Should().BeEquivalentTo(Result.Fail<IFileReader>("File any/path does not exist"));
    }

    [Test]
    public void GetFileReader_ShouldThrowNotSupportedException_WhenFileIsNotSupported()
    {
        var fileReaderFactory = Scope.Resolve<FileReaderFactory>();
        var action = fileReaderFactory.GetFileReader("Utilities\\mystem.exe");
        
        action.Should().BeEquivalentTo(Result.Fail<IFileReader>("File type .exe is not supported."));
    }
}