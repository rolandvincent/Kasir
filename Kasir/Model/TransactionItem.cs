using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kasir.Model
{
    public class TransactionItem
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public Guid TransactionID { get; set; }
        [Required]
        public int ProductID { get; set; }
        [Required]
        public int Qty { get; set; }
        [Required]
        public long ItemPrice { get; set; }
        public long? ItemDiscount { get; set; }

    }
}
