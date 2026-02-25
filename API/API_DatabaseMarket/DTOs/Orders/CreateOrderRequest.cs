namespace API_DatabaseMarket.DTOs.Orders
{
    public record CreateOrderRequest(
        List<CreateOrderItemDto> Items
    );
}