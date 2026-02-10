using System.Text.Json;

namespace API_DatabaseMarket.DTOs.Orders
{
    public class OrderResponse
    {
        public int Id { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }

        public List<OrderItemResponse> Items { get; set; }
    }

    public class OrderItemResponse
    {
        public int Id { get; set; }
        public JsonElement Config { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
