using CommandLine;
using TagsCloudVisualization.ConsoleCommands.Attributes;
using TagsCloudVisualization.Enums;

namespace TagsCloudVisualization.ConsoleCommands;

public class Options
{
    [Option('i', "inputFilePath", Required = true, 
        HelpText = "Set path to a file containing words in one column (under one word per row).")]
    public string InputFilePath { get; init; }

    [Option('o', "outputDirectory", Required = true, 
        HelpText = "Set directory for output image.")]
    public string OutputDirectory { get; init; }

    [Option('f', "font", Default = "Arial", HelpText = "Set font for tags cloud words.")]
    [ValidFontName]
    public string TagsFont { get; init; }
    
    [Option("minFontSize", Default = 5, HelpText = "Set min font size for tags cloud words.")]
    [GreaterThanZero]
    public int MinTagsFontSize { get; init; }
    
    [Option("maxFontSize", Default = 25, HelpText = "Set max font size for tags cloud words.")]
    [GreaterThanZero]
    public int MaxTagsFontSize { get; init; }

    [Option('h', "imageHeight", Default = 1080, HelpText = "Set output image height.")]
    [GreaterThanZero]
    public int ImageHeight { get; init; }
    
    [Option('w', "imageWidth", Default = 1920, HelpText = "Set output image width.")]
    [GreaterThanZero]
    public int ImageWidth { get; init; }
    
    [Option("imageFormat", Default = OutputImageFormat.Png, 
        HelpText = "Set output image format. Possible values: Jpeg, Jpg, Png, Tiff, Bmp, Gif, Pdf.")]
    public OutputImageFormat ImageFormat { get; init; }

    [Option('b', "backgroundColor", Default = "Empty", 
        HelpText = "Set background color for tags cloud. Example : -b white")]
    [ValidColorName]
    public string BackgroundColor { get; init; }
    
    [Option("pathToMyStem", Default = null, HelpText = "Set path to mystem.exe.")]
    public string PathToMyStem { get; init; }
    
    [Option("numOfColors", Default = 85, HelpText = "Set number of colors for gradient color generator.")]
    [GreaterThanZero]
    public int NumOfColors { get; init; }
    
    [Option("colorOption", Default = ColorOption.Random, 
        HelpText = "Set option of color generator for words. Possible values: Random, Gradient.")]
    public ColorOption ColorOption { get; init; }
}