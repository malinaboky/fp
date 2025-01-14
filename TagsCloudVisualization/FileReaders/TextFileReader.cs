using TagsCloudVisualization.FuncMonad;

namespace TagsCloudVisualization.FileReaders;

public class TextFileReader : IFileReader
{
    public Result<string> Read(string filePath)
    {
        return Result.Of(() => File.ReadAllText(filePath));
    }
}