using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kasir.Model
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public int CategoryID { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Barcode { get; set; }
        public string? Notes { get; set; }
        [Required]
        public long Price { get; set; }
        public long? PromoPrice {  get; set; }
    }
}
