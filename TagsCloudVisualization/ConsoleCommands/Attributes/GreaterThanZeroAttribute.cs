using System.ComponentModel.DataAnnotations;

namespace TagsCloudVisualization.ConsoleCommands.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class GreaterThanZeroAttribute : ValidationAttribute 
{
    protected override ValidationResult IsValid(object value, ValidationContext context)
    {
        if (value is not int valueAsInt)
            return new ValidationResult($"Option '{context.DisplayName}' with value {value} must be int.");

        return valueAsInt <= 0 
            ? new ValidationResult($"Option '{context.DisplayName}' with value {valueAsInt} must be greater than zero.") 
            : ValidationResult.Success;
    }
}