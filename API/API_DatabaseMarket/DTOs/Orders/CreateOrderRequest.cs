namespace API_DatabaseMarket.DTOs.Orders
{
    public record CreateOrderRequest(
        decimal TotalAmount,
        List<CreateOrderItemDto> Items
    );
}
