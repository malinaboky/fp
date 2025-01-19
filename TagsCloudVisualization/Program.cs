using System.ComponentModel.DataAnnotations;
using Autofac;
using CommandLine;
using CommandLine.Text;
using TagsCloudVisualization.App;
using TagsCloudVisualization.FuncMonad;
using Options = TagsCloudVisualization.ConsoleCommands.Options;

namespace TagsCloudVisualization;

public class Program
{
    static void Main(string[] args)
    {
        var parserResult = Parser.Default.ParseArguments<Options>(args);
        
        parserResult.WithParsed(opts =>
            {
                if (!TryValidateOptions(opts, out var errors))
                {
                    var helpText = HelpText.AutoBuild(parserResult, h => h, e => e)
                        .AddPreOptionsLines(errors);
                    Console.WriteLine(helpText);
                    Environment.Exit(2);
                }
                
                var container = ContainerConfig.Configure(opts);
                using var scope = container.BeginLifetimeScope();
                scope.Resolve<IApp>()
                    .Run()
                    .OnFail(error =>
                    {
                        Console.WriteLine(error);
                        Environment.Exit(1);
                    });
            })
            .WithNotParsed(_ => Environment.Exit(2));
    }

    private static bool TryValidateOptions(Options options, out List<string> errors)
    {
        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(options);
        errors = [];

        if (Validator.TryValidateObject(options, validationContext, validationResults, true)) 
            return true;

        errors.Add("\nERROR(S):");
        errors.AddRange(validationResults.Select(validationResult => $"  {validationResult.ErrorMessage}"));

        return false;
    }
}