using System.Drawing;
using Autofac;
using TagsCloudVisualization;
using TagsCloudVisualization.BitmapProcessors;
using TagsCloudVisualization.ConsoleCommands;
using TagsCloudVisualization.Enums;

namespace TagsCloudVisualizationTests;

[TestFixture]
public class BitmapProcessorTests
{
    private ILifetimeScope Scope { get; set; }
    private string directory = Path.GetFullPath("rendered");

    [SetUp]
    public void Setup()
    {
        var options = new Options
        {
            InputFilePath = directory,
            OutputDirectory = "/path/in/test/not/needed",
        };
        var container = ContainerConfig.Configure(options);
        Directory.CreateDirectory(directory);
        Scope = container.BeginLifetimeScope();
    }

    [TearDown]
    public void TearDown()
    {
        Scope.Dispose();
        
        if (Directory.Exists(directory))
            Directory.Delete(directory, true);
    }
    
    [TestCase(OutputImageFormat.Jpeg,".jpeg")]
    [TestCase(OutputImageFormat.Jpg, ".jpg")]
    [TestCase(OutputImageFormat.Png, ".png")]
    [TestCase(OutputImageFormat.Gif, ".gif")]
    [TestCase(OutputImageFormat.Bmp, ".bmp")]
    [TestCase(OutputImageFormat.Tiff, ".tiff")]
    [TestCase(OutputImageFormat.Pdf, ".pdf")]
    public void Read_ShouldReturnCorrectStringFromFile(OutputImageFormat format, string expected)
    {
        var imageName = "output";
        var expectedPath = Path.Combine(directory, $"{imageName}.{expected}");
        var bitmapFactory = Scope.Resolve<BitmapProcessorFactory>();

        bitmapFactory.GetBitmapProcessor(format).SaveImage(new Bitmap(1, 1), directory, imageName);
        
        File.Exists(expectedPath);
    }
}