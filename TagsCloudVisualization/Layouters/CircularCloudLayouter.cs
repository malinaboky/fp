using System.Drawing;
using TagsCloudVisualization.Distributors;
using TagsCloudVisualization.Domain;
using TagsCloudVisualization.FuncMonad;
using TagsCloudVisualization.Layouters.RectangleSizeCalculators;
using TagsCloudVisualization.WordPreprocessors.FontCreators;

namespace TagsCloudVisualization.Layouters;

public class CircularCloudLayouter : ICloudLayouter
{
    private readonly ICloudDistribution distribution;
    private readonly IFontCreator fontCreator;
    private readonly IRectangleSizeCalculator rectangleSizeCalculator;
    
    public CircularCloudLayouter(ICloudDistribution distribution, 
        IFontCreator fontCreator,
        IRectangleSizeCalculator rectangleSizeCalculator)
    {
        this.distribution = distribution;
        this.fontCreator = fontCreator;
        this.rectangleSizeCalculator = rectangleSizeCalculator;
    }
    
    public Result<IEnumerable<Tag>> CreateTagsCloud(IEnumerable<Tuple<string, int>> wordsCollection)
    {
        return wordsCollection.Any() 
            ? FillLayouter(wordsCollection)
            : Result.Fail<IEnumerable<Tag>>("List of words is empty after preprocessing the words.");
    }
    
    private Result<Rectangle> GetNextRectangle(List<Tag> tags, Size rectangleSize)
    {
        if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
            return Result.Fail<Rectangle>("The rectangle size must be greater than zero.");
        
        var newRectangle = new Rectangle(distribution.GetNextPoint(), rectangleSize);
        
        while (tags.Any(r => r.Rectangle.IntersectsWith(newRectangle)))
            newRectangle.Location = distribution.GetNextPoint();
        
        return newRectangle;
    }

    private Result<IEnumerable<Tag>> FillLayouter(IEnumerable<Tuple<string, int>> wordsCollection)
    {
        List<Tag> tags = [];
        var maxWordCount = wordsCollection.First().Item2;
        var minWordCount = wordsCollection.Last().Item2;
       
        foreach (var (word, wordCount) in wordsCollection)
        {
            var tagFont = fontCreator.CreateFont(wordCount, minWordCount, maxWordCount);
            var rectangle = tagFont
                .Then(font => rectangleSizeCalculator.ConvertWordToRectangleSize(word, font))
                .Then(size => GetNextRectangle(tags, size));
            
            if (!rectangle.IsSuccess)
                return Result.Fail<IEnumerable<Tag>>(rectangle.Error);
                
            tags.Add(new Tag(rectangle.GetValueOrThrow(), tagFont.GetValueOrThrow(), word));
        }
        
        return tags;
    }
}