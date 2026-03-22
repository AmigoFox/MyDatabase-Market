using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace API_DatabaseMarket.DTOs
{
    public class OrderItemDto
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int OrderId { get; set; }

        [Required]
        public JsonElement Config { get; set; }
    }
}
