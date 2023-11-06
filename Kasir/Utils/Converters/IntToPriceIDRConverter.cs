using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Kasir.Utils.Converters
{
    public class IntToPriceIDRConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int price)
            {
                string formattedPrice = string.Format("Rp{0:N0}", price);
                return formattedPrice;
            }else if(value is long priceLong)
            {
                string formattedPrice = string.Format("Rp{0:N0}", priceLong);
                return formattedPrice;
            }

            return "Rp0";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
