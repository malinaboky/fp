using Autofac;
using FluentAssertions;
using TagsCloudVisualization;
using TagsCloudVisualization.BitmapProcessors;
using TagsCloudVisualization.ConsoleCommands;
using TagsCloudVisualization.Enums;

namespace TagsCloudVisualizationTests;

[TestFixture]
public class BitmapProcessorFactoryTests
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
    
    [TestCase(OutputImageFormat.Jpeg, typeof(DefaultBitmapProcessor))]
    [TestCase(OutputImageFormat.Jpg, typeof(DefaultBitmapProcessor))]
    [TestCase(OutputImageFormat.Png, typeof(DefaultBitmapProcessor))]
    [TestCase(OutputImageFormat.Gif, typeof(DefaultBitmapProcessor))]
    [TestCase(OutputImageFormat.Bmp, typeof(DefaultBitmapProcessor))]
    [TestCase(OutputImageFormat.Tiff, typeof(DefaultBitmapProcessor))]
    [TestCase(OutputImageFormat.Pdf, typeof(PdfBitmapProcessor))]
    public void GetBitmapProcessor_ShouldReturnCorrectBitmapProcessor(OutputImageFormat option, Type expectedType)
    {
        var bitmapFactory = Scope.Resolve<BitmapProcessorFactory>();

        bitmapFactory.GetBitmapProcessor(option).Should().BeOfType(expectedType);
    }
}