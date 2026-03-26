using System.Text.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API_DatabaseMarket.DTOs
{
    public class OrderItemDto
    {
        public int? OrderId { get; set; }

        [Required]
        public JsonElement Config { get; set; }

        [Required]
        public string DatabaseType { get; set; } = default!;

        [Required]
        public int SizeGB { get; set; }

        [Required]
        public string Iops { get; set; } = default!;

        [Required]
        public string StorageType { get; set; } = default!;

        [Required]
        public string Scalability { get; set; } = default!;

        [Required]
        public decimal FinalPriceRub { get; set; }

        public string? OrderName { get; set; }

        public List<string>? Countries { get; set; } = new();
    }
}
