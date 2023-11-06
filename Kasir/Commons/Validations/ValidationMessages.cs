using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kasir.Commons.Validations
{
    public class ValidationMessages
    {
        public const string RequiredError = "* Kolom {0} tidak boleh kosong!";
        public const string NumericError = "* Kolom {0} harus berisi angka!";
        public const string MinMaxError = "* Kolom {0} harus berisi angka antara {1} dan {2}!";
    }
}
