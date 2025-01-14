using Autofac;
using TagsCloudVisualization.App;
using TagsCloudVisualization.BitmapProcessors;
using TagsCloudVisualization.ConsoleCommands;
using TagsCloudVisualization.Distributors;
using TagsCloudVisualization.Enums;
using TagsCloudVisualization.FileReaders;
using TagsCloudVisualization.Layouters;
using TagsCloudVisualization.Layouters.RectangleSizeCalculators;
using TagsCloudVisualization.MyStemWrapper;
using TagsCloudVisualization.Renderers;
using TagsCloudVisualization.Renderers.ColorGenerators;
using TagsCloudVisualization.WordPreprocessors;
using TagsCloudVisualization.WordPreprocessors.FontCreators;
using TagsCloudVisualization.WordPreprocessors.WordValidators;

namespace TagsCloudVisualization;

public static class ContainerConfig
{
    public static IContainer Configure(Options options)
    {
        var builder = new ContainerBuilder();
        builder.RegisterInstance(options).AsSelf().SingleInstance();
        builder.RegisterType<FileReaderFactory>().AsSelf();
        builder.RegisterType<ColorGeneratorFactory>().AsSelf();
        builder.RegisterType<BitmapProcessorFactory>().AsSelf();
        builder.RegisterType<SpiralDistribution>().As<ICloudDistribution>();
        builder.RegisterType<DefaultRenderer>().As<ICloudRenderer>();
        builder.RegisterType<DefaultFontCreator>().As<IFontCreator>();
        builder.RegisterType<DefaultWordValidator>().As<IWordValidator>();
        builder.RegisterType<DefaultRectangleSizeCalculator>().As<IRectangleSizeCalculator>();
        builder.RegisterType<DefaultWordPreprocessor>().As<IWordPreprocessor>();
        builder.RegisterType<CircularCloudLayouter>().As<ICloudLayouter>();
        builder.RegisterType<ConsoleApp>().As<IApp>();
        ConfigureFileReaders(builder);
        ConfigureColorGenerators(builder);
        ConfigureBitmapProcessors(builder);

        var myStem = new MyStem
        {
            PathToMyStem = options.PathToMyStem ?? Path.GetFullPath("Utilities\\mystem.exe"),
            Parameters = "-nig --format json"
        };
        builder.RegisterInstance(myStem).AsSelf().SingleInstance();
        
        return builder.Build();
    }

    private static void ConfigureFileReaders(ContainerBuilder builder)
    {
        builder.RegisterType<TextFileReader>().Keyed<IFileReader>(".txt");
        builder.RegisterType<DocxFileReader>().Keyed<IFileReader>(".docx");
        builder.RegisterType<DocFileReader>().Keyed<IFileReader>(".doc");
    }
    
    private static void ConfigureColorGenerators(ContainerBuilder builder)
    {
        builder.RegisterType<GradientColorGenerator>().Keyed<IColorGenerator>(ColorOption.Gradient);
        builder.RegisterType<DefaultColorGenerator>().Keyed<IColorGenerator>(ColorOption.Random);
    }
    
    private static void ConfigureBitmapProcessors(ContainerBuilder builder)
    {
        builder.RegisterType<DefaultBitmapProcessor>()
            .Keyed<IBitmapProcessor>(OutputImageFormat.Jpg)
            .Keyed<IBitmapProcessor>(OutputImageFormat.Png)
            .Keyed<IBitmapProcessor>(OutputImageFormat.Gif)
            .Keyed<IBitmapProcessor>(OutputImageFormat.Tiff)
            .Keyed<IBitmapProcessor>(OutputImageFormat.Bmp)
            .Keyed<IBitmapProcessor>(OutputImageFormat.Jpeg);
        builder.RegisterType<PdfBitmapProcessor>()
            .Keyed<IBitmapProcessor>(OutputImageFormat.Pdf);
    }
}