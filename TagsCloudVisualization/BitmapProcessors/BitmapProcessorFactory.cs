using Autofac;
using TagsCloudVisualization.Enums;

namespace TagsCloudVisualization.BitmapProcessors;

public class BitmapProcessorFactory
{
    private readonly IComponentContext context;
    
    public BitmapProcessorFactory(IComponentContext context)
        => this.context = context;

    public IBitmapProcessor GetBitmapProcessor(OutputImageFormat option)
    {
        if (context.IsRegisteredWithKey<IBitmapProcessor>(option))
            return context.ResolveKeyed<IBitmapProcessor>(option);

        throw new NotSupportedException($"Image format {option.ToString()} is not supported.");
    }
}