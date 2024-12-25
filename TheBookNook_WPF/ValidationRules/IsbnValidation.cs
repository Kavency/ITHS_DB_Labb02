using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace TheBookNook_WPF.ValidationRules
{

    public class IsbnValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo) 
        { 
            string input = value as string; 
            if (string.IsNullOrWhiteSpace(input)) 
            { 
                return new ValidationResult(false, "ISBN cannot be empty."); 
            } 
            Regex regex = new Regex(@"^\d{10}(\d{3})?$"); 
            if (!regex.IsMatch(input)) 
            { 
                return new ValidationResult(false, "ISBN must be 10 or 13 digits.");
            } 
            return ValidationResult.ValidResult; 
        }
    }
}
