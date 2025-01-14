using TagsCloudVisualization.FuncMonad;

namespace TagsCloudVisualization.App;

public interface IApp
{
    public Result<None> Run();
}