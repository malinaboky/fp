using Autofac;
using TagsCloudVisualization.Enums;
using TagsCloudVisualization.FuncMonad;

namespace TagsCloudVisualization.Renderers.ColorGenerators;

public class ColorGeneratorFactory
{
    private readonly IComponentContext context;
    
    public ColorGeneratorFactory(IComponentContext context)
        => this.context = context;

    public Result<IColorGenerator> GetColorGenerator(ColorOption option)
    {
        return context.IsRegisteredWithKey<IColorGenerator>(option) 
            ? Result.Ok(context.ResolveKeyed<IColorGenerator>(option)) 
            : Result.Fail<IColorGenerator>($"Color generator {option.ToString()} is not supported.");
    }
}