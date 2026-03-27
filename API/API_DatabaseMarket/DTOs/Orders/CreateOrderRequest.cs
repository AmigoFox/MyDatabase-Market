namespace API_DatabaseMarket.DTOs.Orders
{
    public class CreateOrderRequest
    {
        public List<CreateOrderItemDto> Items { get; set; } = new();
        public string? OrderName { get; set; }
    }
}