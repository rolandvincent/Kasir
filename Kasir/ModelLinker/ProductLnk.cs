using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kasir.ModelLinker
{
    public class ProductLnk
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public string ProductName { get; set; }
        public string Barcode { get; set; }
        public string? Notes { get; set; }
        public int Stock { get; set; }
        public long Price { get; set; }
        public long? PromoPrice { get; set; }
    }
}
