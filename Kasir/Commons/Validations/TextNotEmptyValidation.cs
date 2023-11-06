using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Kasir.Commons.Validations
{
    public class TextNotEmptyValidation : ValidationRule
    {
        public string FieldName { get; set; } = "Field";
        public string CustomErrorMessage { get; set; } = string.Empty;
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (string.IsNullOrWhiteSpace(value?.ToString()))
                return new ValidationResult(false, string.Format(string.IsNullOrEmpty(CustomErrorMessage) ? "{0} cannot be empty" : CustomErrorMessage, FieldName));
            return ValidationResult.ValidResult;
        }
    }
}
