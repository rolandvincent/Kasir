using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Kasir.Commons.Validations
{
    public class IntegerValidationRule : ValidationRule
    {
        private int _min = int.MinValue;
        private int _max = int.MaxValue;
        private string _fieldName = "Field";
        private string _minMaxErrorMessage = String.Empty;
        private string _typeErrorMessage = String.Empty;

        public int Min
        {
            get { return _min; }
            set { _min = value; }
        }

        public int Max
        {
            get { return _max; }
            set { _max = value; }
        }

        public string FieldName
        {
            get { return _fieldName; }
            set { _fieldName = value; }
        }

        /// <summary>
        /// Custom error message for min or max
        /// {0} Name Field
        /// {1} Min
        /// {2} Max
        /// </summary>
        public string MinMaxErrorMessage
        {
            get { return _minMaxErrorMessage; }
            set { _minMaxErrorMessage = value; }
        }

        /// <summary>
        ///  Custom error for input if not Integer
        ///  {0} Name Field
        /// </summary>
        public string TypeErrorMessage
        {
            get { return _typeErrorMessage; }
            set { _typeErrorMessage = value; }
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int num = 0;

            if (!int.TryParse(value.ToString(), out num))
                return new ValidationResult(false, String.Format(string.IsNullOrEmpty(TypeErrorMessage) ? "{0} must contain an integer value." : TypeErrorMessage, FieldName));

            if (num < Min || num > Max)
            {
                if (!String.IsNullOrEmpty(MinMaxErrorMessage))
                    return new ValidationResult(false, String.Format(MinMaxErrorMessage,
                                           FieldName, Min, Max));

                return new ValidationResult(false, String.Format("{0} must be between {1} and {2}.",
                                           FieldName, Min, Max));
            }

            return new ValidationResult(true, null);
        }
    }
}
