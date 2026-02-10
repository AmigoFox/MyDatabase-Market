using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace API_DatabaseMarket.DTOs.Orders
{
    public class CreateOrderItemDto
    {
        [Required]
        public JsonElement Config { get; set; }
    }
}
