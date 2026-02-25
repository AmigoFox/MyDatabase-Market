using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace API_DatabaseMarket.Models
{
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Order))]
        public int OrderId { get; set; }
        public string DatabaseType { get; set; } = default!;
        public int SizeGB { get; set; }
        public string Iops { get; set; } = default!;
        public string StorageType { get; set; } = default!;
        public string Scalability { get; set; } = default!;
        public decimal FinalPriceRub { get; set; }

        public JsonElement Config { get; set; }

        public DateTime CreatedAt { get; set; }

        public Order Order { get; set; } = default!;
        public ICollection<OrderItemCountry> Countries { get; set; } = new List<OrderItemCountry>();
    }
}
