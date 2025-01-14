using Autofac;
using FluentAssertions;
using TagsCloudVisualization;
using TagsCloudVisualization.ConsoleCommands;
using TagsCloudVisualization.Enums;
using TagsCloudVisualization.Renderers.ColorGenerators;

namespace TagsCloudVisualizationTests;

[TestFixture]
public class ColorGeneratorFactoryTests
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
    
    [TestCase(ColorOption.Random, typeof(DefaultColorGenerator))]
    [TestCase(ColorOption.Gradient, typeof(GradientColorGenerator))]
    public void GetColorGenerator_ShouldReturnCorrectColorGenerator(ColorOption option, Type expectedType)
    {
        var colorGeneratorFactory = Scope.Resolve<ColorGeneratorFactory>();

        colorGeneratorFactory.GetColorGenerator(option).Should().BeOfType(expectedType);
    }
}