using Autofac;
using TagsCloudVisualization.Enums;
using TagsCloudVisualization.FuncMonad;

namespace TagsCloudVisualization.BitmapProcessors;

public class BitmapProcessorFactory
{
    private readonly IComponentContext context;
    
    public BitmapProcessorFactory(IComponentContext context)
        => this.context = context;

    public Result<IBitmapProcessor> GetBitmapProcessor(OutputImageFormat option)
    {
        return context.IsRegisteredWithKey<IBitmapProcessor>(option) 
            ? Result.Ok(context.ResolveKeyed<IBitmapProcessor>(option)) 
            : Result.Fail<IBitmapProcessor>($"Image format {option.ToString()} is not supported.");
    }
}