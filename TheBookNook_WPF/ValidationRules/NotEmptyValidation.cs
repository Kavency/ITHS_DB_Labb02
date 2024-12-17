using System.Globalization;
using System.Windows.Controls;

namespace TheBookNook_WPF.ValidationRules
{
    internal class NotEmptyValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if(string.IsNullOrWhiteSpace(value as string))
                return new ValidationResult(false, "Field cannot be empty.");

            return ValidationResult.ValidResult;
        }
    }
}
