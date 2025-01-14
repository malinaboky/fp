using TagsCloudVisualization.ConsoleCommands;
using TagsCloudVisualization.FileReaders;
using TagsCloudVisualization.FuncMonad;
using TagsCloudVisualization.Layouters;
using TagsCloudVisualization.Renderers;
using TagsCloudVisualization.WordPreprocessors;

namespace TagsCloudVisualization.App;

public class ConsoleApp : IApp
{
    private readonly IWordPreprocessor wordPreprocessor;
    private readonly FileReaderFactory fileReaderFactory;
    private readonly ICloudLayouter cloudLayouter;
    private readonly ICloudRenderer cloudRenderer;
    private readonly string inputFilePath;

    public ConsoleApp(IWordPreprocessor wordPreprocessor,
        FileReaderFactory fileReaderFactory,
        ICloudLayouter cloudLayouter,
        ICloudRenderer cloudRenderer,
        Options options)
    {
        this.wordPreprocessor = wordPreprocessor;
        this.fileReaderFactory = fileReaderFactory;
        this.cloudLayouter = cloudLayouter;
        this.cloudRenderer = cloudRenderer;
        inputFilePath = options.InputFilePath;
    }
    
    public Result<None> Run()
    {
        return fileReaderFactory
            .GetFileReader(inputFilePath)
            .Then(reader => reader.Read(inputFilePath))
            .Then(wordPreprocessor.ProcessTextToWords)
            .Then(cloudLayouter.CreateTagsCloud)
            .Then(cloudRenderer.Render);
    }
}