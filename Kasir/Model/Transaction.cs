using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kasir.Model
{
    public class Transaction
    {
        [Key]
        public Guid Id { get; set; }
        public int? CusomerID { get; set; }
        [Required]
        public long TotalPrice { get; set; }
        [Required]
        public long TotalPayment { get; set; }
        [Required]
        public DateTime DateTransaction { get; set; }
        public string? Notes { get; set; }
    }
}
