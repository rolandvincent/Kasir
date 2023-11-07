using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Kasir.Commons.Validations
{
    public class NullValidationRule : ValidationRule
    {
        public string FieldName { get; set; } = "Field";

		private string _customError = string.Empty; 
		/// <summary>
		/// Custom error if null
		/// {0} Field Name
		/// </summary>
		public string CustomErrorMessage
		{
			get
			{
				return _customError;
			}
			set
			{
				_customError = value;
			}
		}
		public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null)
				return new ValidationResult(false, string.IsNullOrEmpty(CustomErrorMessage) ? FieldName + " cannot be null" : string.Format(CustomErrorMessage, FieldName));
			return ValidationResult.ValidResult;
        }
    }
}
