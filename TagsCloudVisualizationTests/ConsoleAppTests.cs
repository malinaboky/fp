using System.Drawing;
using Autofac;
using TagsCloudVisualization;
using TagsCloudVisualization.App;
using TagsCloudVisualization.ConsoleCommands;
using TagsCloudVisualization.Domain;
using TagsCloudVisualization.Enums;
using TagsCloudVisualization.FileReaders;
using TagsCloudVisualization.Layouters;
using TagsCloudVisualization.Renderers;
using TagsCloudVisualization.WordPreprocessors;
using Telerik.JustMock;

namespace TagsCloudVisualizationTests;

[TestFixture]
public class ConsoleAppTests
{
    private ConsoleApp ConsoleApp { get; set; }
    private ILifetimeScope Scope { get; set; }
    private IWordPreprocessor WordPreprocessor { get; set; }
    private ICloudLayouter Layouter { get; set; }
    private Options Options { get; set; }
    
    [SetUp]
    public void SetUp()
    {
        SetUpScope();
        WordPreprocessor = Mock.Create<IWordPreprocessor>();
        Layouter = Mock.Create<ICloudLayouter>();
        ConsoleApp = new ConsoleApp(
            WordPreprocessor,
            Scope.Resolve<FileReaderFactory>(),
            Layouter,
            Scope.Resolve<ICloudRenderer>(),
            Options
        );
    }
    
    [TearDown]
    public void TearDown()
    {
        Scope.Dispose();
        
        if (Directory.Exists(Options.OutputDirectory))
            Directory.Delete(Options.OutputDirectory, true);
    }

    private void SetUpScope()
    {
        Options = new Options
        {
            InputFilePath = Path.GetFullPath("source\\input.txt"),
            OutputDirectory = Path.GetFullPath("rendered"),
            ColorOption = ColorOption.Random,
            ImageFormat = OutputImageFormat.Png,
            ImageWidth = 50,
            ImageHeight = 50,
            BackgroundColor = "#FF0000"
        };
        Directory.CreateDirectory(Options.OutputDirectory);
        var container = ContainerConfig.Configure(Options);
        Scope = container.BeginLifetimeScope();
    }

    [Test]
    public void Run_ShouldSaveFileToOutputDirectory()
    {
        Mock.Arrange(() => WordPreprocessor.ProcessTextToWords(Arg.AnyString))
            .Returns(() => new List<Tuple<string, int>>());
        Mock.Arrange(() => Layouter.CreateTagsCloud(Arg.IsAny<IEnumerable<Tuple<string, int>>>()))
            .Returns(
                new List<Tag>
                {
                    new(new Rectangle(new Point(0, 0), new Size(50, 50)),
                        new Font("Arial", 1),
                        "fake")
                }
            );
        
        ConsoleApp.Run();
        
        File.Exists(Path.GetFullPath("rendered\\cloud_1.png"));
    }
}