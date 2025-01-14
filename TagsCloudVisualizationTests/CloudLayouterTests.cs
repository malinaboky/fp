using System.Drawing;
using FluentAssertions;
using TagsCloudVisualization.ConsoleCommands;
using TagsCloudVisualization.Distributors;
using TagsCloudVisualization.Domain;
using TagsCloudVisualization.Layouters;
using TagsCloudVisualization.Layouters.RectangleSizeCalculators;
using TagsCloudVisualization.WordPreprocessors.FontCreators;
using Telerik.JustMock;
using Telerik.JustMock.Helpers;

namespace TagsCloudVisualizationTests;

[TestFixture]
public class CloudLayouterTests
{
    private ICloudLayouter Layouter { get; set; }
    private IFontCreator FakeFontCreator { get; set; }
    private IRectangleSizeCalculator FakeRectangleSizeCalculator { get; set; }
    private Size imageSize = new(50, 50);

    [SetUp]
    public void Setup()
    {
        FakeFontCreator = Mock.Create<IFontCreator>();
        FakeRectangleSizeCalculator = Mock.Create<IRectangleSizeCalculator>();
        
        Layouter = new CircularCloudLayouter(
                new SpiralDistribution(new Options {ImageWidth = imageSize.Width, ImageHeight = imageSize.Height}),
                FakeFontCreator,
                FakeRectangleSizeCalculator
            );
    }

    [Test]
    public void CreateTagsCloud_ShouldReturnCorrectFirstTag()
    {
        Mock.Arrange(() => FakeFontCreator.CreateFont(Arg.AnyInt, Arg.AnyInt, Arg.AnyInt))
            .Returns(new Font("Arial", 12));
        Mock.Arrange(() => FakeRectangleSizeCalculator.ConvertWordToRectangleSize(Arg.AnyString, Arg.IsAny<Font>()))
            .Returns(new Size(10, 10));

        Layouter.CreateTagsCloud([new Tuple<string, int>("text", 10)])
            .GetValueOrThrow()
            .First()
            .Should().BeEquivalentTo(new Tag(
                new Rectangle(new Point(imageSize.Width / 2, imageSize.Height / 2), new Size(10, 10)), 
                new Font("Arial", 12),
                "text"));
    }
    
    [Test]
    public void CreateTagsCloud_ShouldReturnFirstTagInImageCenter()
    {
        Mock.Arrange(() => FakeFontCreator.CreateFont(Arg.AnyInt, Arg.AnyInt, Arg.AnyInt))
            .Returns(new Font("Arial", 12, FontStyle.Regular));
        Mock.Arrange(() => FakeRectangleSizeCalculator.ConvertWordToRectangleSize(Arg.AnyString, Arg.IsAny<Font>()))
            .Returns(new Size(10, 10));

        Layouter.CreateTagsCloud([new Tuple<string, int>("text", 10)])
            .GetValueOrThrow()
            .First()
            .Rectangle.Location
            .Should().Be(new Point(imageSize.Width / 2, imageSize.Height / 2));
    }
    
    [TestCase(16)]
    [TestCase(64)]
    [TestCase(256)]
    public void CreateTagsCloud_GeneratesTagsWithoutIntersects(int wordCount)
    {
        Mock.Arrange(() => FakeFontCreator.CreateFont(Arg.AnyInt, Arg.AnyInt, Arg.AnyInt))
            .Returns(new Font("Arial", 12, FontStyle.Regular));
        Mock.Arrange(() => FakeRectangleSizeCalculator.ConvertWordToRectangleSize(Arg.AnyString, Arg.IsAny<Font>()))
            .ReturnsMany(Enumerable.Range(0, wordCount)
                .Select(_ => new Size(10, 10))
                .ToArray());

        var tags = Layouter.CreateTagsCloud(
            Enumerable.Range(0, wordCount).Select(_ => new Tuple<string, int>("text", 10)));
        
        HasIntersectedRectangles(tags.GetValueOrThrow().ToList()).Should().Be(false);
    }
    
    private static bool HasIntersectedRectangles(List<Tag> tags)
    {
        for (var i = 0; i < tags.Count - 1; i++)
        {
            for (var j = i + 1; j < tags.Count; j++)
            {
                if (tags[i].Rectangle.IntersectsWith(tags[j].Rectangle))
                    return true;
            }
        }
        return false;
    }
}