using TagsCloudVisualization.FuncMonad;

namespace TagsCloudVisualization.FileReaders;

public interface IFileReader
{
    public Result<string> Read(string filePath);
}