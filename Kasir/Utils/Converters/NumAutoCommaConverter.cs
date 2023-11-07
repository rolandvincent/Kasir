using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Kasir.Utils.Converters
{
    public class NumAutoCommaConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int price)
                return string.Format(culture, "{0:N0}", price);
            else if (value is long priceLong)
                return string.Format(culture, "{0:N0}", priceLong);

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString())) return null;
            return long.TryParse(value?.ToString().Replace(",","").Replace(".", ""), out long result) ? result : 0;
        }
    }
}
