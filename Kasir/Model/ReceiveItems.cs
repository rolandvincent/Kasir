using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kasir.Model
{
    public class ReceiveItems
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int ProductID {  get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public long PurchasePrice { get; set; }
    }
}
