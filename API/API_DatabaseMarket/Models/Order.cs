using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_DatabaseMarket.Models
{
    public class Order
    {
        [Key]   
        public int Id { get; set; }

        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        public decimal TotalAmount { get; set; }

        [MaxLength(50)]
        public string Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }


        [MaxLength(150)]
        public string OrderName { get; set; } = "Новый заказ";

        public DateTime? PaymentDueDate { get; set; }


        // Navigation
        public User? User { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}
