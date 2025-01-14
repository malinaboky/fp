using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Autofac;
using CommandLine;
using TagsCloudVisualization.App;
using TagsCloudVisualization.FuncMonad;
using Options = TagsCloudVisualization.ConsoleCommands.Options;

namespace TagsCloudVisualization;

public class Program
{
    static void Main(string[] args)
    {
        Parser.Default.ParseArguments<Options>(args)
            .WithParsed(opts =>
            {
                if (!IsValidOptions(opts))
                    return;
                
                Result.OfAction(() => 
                    { 
                        var container = ContainerConfig.Configure(opts);
                        using var scope = container.BeginLifetimeScope();
                        scope.Resolve<IApp>().Run();
                    })
                    .OnFail(Console.WriteLine);
            });
    }

    private static bool IsValidOptions(Options options)
    {
        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(options);

        if (Validator.TryValidateObject(options, validationContext, validationResults, true)) 
            return true;

        Console.WriteLine("ERROR(S):");
        
        foreach (var validationResult in validationResults)
            Console.WriteLine($"  {validationResult.ErrorMessage}");
        
        PrintHelp<Options>();
        
        return false;
    }
    
    private static void PrintHelp<T>()
    {
        Console.WriteLine();
        var type = typeof(T);
        var properties = type.GetProperties();

        foreach (var property in properties)
        {
            var optionAttribute = property.GetCustomAttribute<OptionAttribute>();
            
            if (optionAttribute == null) 
                continue;
            
            var shortName = !string.IsNullOrEmpty(optionAttribute.ShortName) 
                ? $"-{optionAttribute.ShortName}, " 
                : string.Empty;
            var longName = $"--{optionAttribute.LongName}";
            var helpText = optionAttribute.HelpText;
            var required = optionAttribute.Required ? "Required. " : string.Empty;
            var defaultValue = optionAttribute.Default != null ? $"(Default: {optionAttribute.Default}) " : string.Empty;

            Console.WriteLine($"  {shortName}{longName}    {required}{defaultValue}{helpText}\n");
        }
    }
}