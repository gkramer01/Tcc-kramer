using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Domain.Extensions
{
    public class FlexibleUrlAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
                return ValidationResult.Success;

            var url = value.ToString()!.Trim();

            // Aceita URLs com ou sem http/https
            var pattern = @"^(https?:\/\/)?([\w\-]+\.)+[\w\-]+(\/[\w\-._~:/?#[\]@!$&'()*+,;=]*)?$";

            if (Regex.IsMatch(url, pattern, RegexOptions.IgnoreCase))
                return ValidationResult.Success;

            return new ValidationResult(ErrorMessage ?? "Invalid URL format.");
        }
    }
}
