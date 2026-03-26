using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace API_DatabaseMarket.DTOs.Orders
{
    public class CreateOrderItemDto
    {
        [Required]
        public string DatabaseType { get; set; } = default!;

        [Required]
        [Range(1, int.MaxValue)]
        public int SizeGB { get; set; }

        [Required]
        public string Iops { get; set; } = default!;

        [Required]
        public string StorageType { get; set; } = default!;

        [Required]
        public string Scalability { get; set; } = default!;

        [Required]
        public string OrderName { get; set; } = default!;

        [Required]
        [MinLength(1)]
        public List<string> Countries { get; set; } = new();
        public JsonElement? Config { get; set; }
    }
}