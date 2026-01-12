using System.Text.Json;

namespace API_DatabaseMarket
{
    namespace API_DatabaseMarket.Models
    {
        public class OrderItemDto
        {
            public int OrderId { get; set; }
            public JsonElement Config { get; set; }
        }

    }
}
