using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CrossApp.Models
{
    public class OrderItemDto
    {
        private OrderItemDto? _order;

        public int Id { get; set; }

        public string DatabaseType { get; set; } = "";

        public int SizeGB { get; set; }

        public string Iops { get; set; } = "";

        public string StorageType { get; set; } = "";

        public string Scalability { get; set; } = "";

        public decimal FinalPriceRub { get; set; }

        public List<string> Countries { get; set; } = new();

        public OrderItemConfigDto Config { get; set; } = new();

        public string OrderName { get; set; } = string.Empty;

        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }
    }
}
