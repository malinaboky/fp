using Autofac;
using TagsCloudVisualization.Enums;

namespace TagsCloudVisualization.Renderers.ColorGenerators;

public class ColorGeneratorFactory
{
    private readonly IComponentContext context;
    
    public ColorGeneratorFactory(IComponentContext context)
        => this.context = context;

    public IColorGenerator GetColorGenerator(ColorOption option)
    {
        if (context.IsRegisteredWithKey<IColorGenerator>(option))
            return context.ResolveKeyed<IColorGenerator>(option);

        throw new NotSupportedException($"Color generator {option.ToString()} is not supported.");
    }
}