using System.ComponentModel.DataAnnotations;
using System.Drawing.Text;

namespace TagsCloudVisualization.ConsoleCommands.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class ValidFontNameAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext context)
    {
        if (value is not string valueAsStr)
            return new ValidationResult($"Option '{context.DisplayName}' with value {value} must be str.");

        var installedFonts = new InstalledFontCollection();
        var fontFamilies = installedFonts.Families;

        return fontFamilies.Any(ff => string.Equals(ff.Name, valueAsStr, StringComparison.OrdinalIgnoreCase)) 
            ? ValidationResult.Success 
            : new ValidationResult($"Option '{context.DisplayName}' with value '{valueAsStr}' is not a valid font name.");
    }
}