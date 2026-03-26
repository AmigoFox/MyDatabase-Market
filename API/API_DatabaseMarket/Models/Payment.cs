using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_DatabaseMarket.Models
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Order))]
        public int OrderId { get; set; }

        public decimal Amount { get; set; }

        [MaxLength(50)]
        public string PaymentMethod { get; set; } = default!;

        [MaxLength(50)]
        public string Status { get; set; } = default!;
        public string TransactionId { get; set; } = default!;

        [MaxLength(255)]
        public DateTime CreatedAt { get; set; }

        // Navigation
        public Order Order { get; set; }
    }
}