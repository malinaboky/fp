using System.Drawing;
using TagsCloudVisualization.BitmapProcessors;
using TagsCloudVisualization.ConsoleCommands;
using TagsCloudVisualization.Domain;
using TagsCloudVisualization.FuncMonad;
using TagsCloudVisualization.Renderers.ColorGenerators;

namespace TagsCloudVisualization.Renderers;

public class DefaultRenderer(ColorGeneratorFactory colorGeneratorFactory, 
    BitmapProcessorFactory bitmapProcessorFactory,
    Options options) : ICloudRenderer
{
    private readonly Result<IColorGenerator> colorGenerator = colorGeneratorFactory.GetColorGenerator(options.ColorOption);
    private readonly Result<IBitmapProcessor> bitmapProcessor = bitmapProcessorFactory.GetBitmapProcessor(options.ImageFormat);
    private readonly string outputDirectory = options.OutputDirectory;
    private readonly Size imageSize = new(options.ImageWidth, options.ImageHeight);
    private readonly Color backgroundColor = Color.FromName(options.BackgroundColor);

    public Result<None> Render(IEnumerable<Tag> tags)
    {
        if (!colorGenerator.IsSuccess)
            return Result.Fail<None>(colorGenerator.Error);
        
        if (!bitmapProcessor.IsSuccess)
            return Result.Fail<None>(bitmapProcessor.Error);
        
        return !tags.Any() 
            ? Result.Fail<None>("The cloud layout is empty")
            : Result.OfAction(() => 
                {
                    using var bitmap = new Bitmap(imageSize.Width, imageSize.Height);
                    CreateBitmap(bitmap, tags);
                    bitmapProcessor.GetValueOrThrow()
                        .SaveImage(bitmap, outputDirectory, $"cloud_{tags.Count()}");
                });
    }
    
    private void CreateBitmap(Bitmap bitmap, IEnumerable<Tag> tags)
    {
        using var graphic = Graphics.FromImage(bitmap);
        
        graphic.Clear(backgroundColor);
        foreach (var tag in tags)
        {
            var color = colorGenerator.GetValueOrThrow().GetColor();
            var brush = new SolidBrush(color);
            graphic.DrawString(tag.Content, tag.Font, brush, tag.Rectangle.Location);
        }
    }
}