using Autofac;
using TagsCloudVisualization.FuncMonad;

namespace TagsCloudVisualization.FileReaders;

public class FileReaderFactory
{
    private readonly IComponentContext context;
    
    public FileReaderFactory(IComponentContext context)
        => this.context = context;

    public Result<IFileReader> GetFileReader(string filePath)
    {
        if (!File.Exists(filePath)) 
            return Result.Fail<IFileReader>($"File {filePath} does not exist");
        
        var extension = Path.GetExtension(filePath).ToLower();

        return context.IsRegisteredWithKey<IFileReader>(extension) 
            ? Result.Ok(context.ResolveKeyed<IFileReader>(extension)) 
            : Result.Fail<IFileReader>($"File type {extension} is not supported.");
    }
}