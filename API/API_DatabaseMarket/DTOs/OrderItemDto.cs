using System.Text.Json;

namespace API_DatabaseMarket.DTOs
{
    public class OrderItemDto
    {
        public int OrderId { get; set; }

        public JsonElement Config { get; set; }
    }
}
