using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace TagsCloudVisualization.ConsoleCommands.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class ValidColorNameAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext context)
    {
        if (value is not string valueAsStr)
            return new ValidationResult($"Option '{context.DisplayName}' with value {value} must be str.");

        var color = Color.FromName(valueAsStr);

        return color.IsKnownColor || string.Equals(valueAsStr, "Empty", StringComparison.OrdinalIgnoreCase)
            ? ValidationResult.Success 
            : new ValidationResult($"Option '{context.DisplayName}' with value '{valueAsStr}' is not a valid color name.");
    }
}