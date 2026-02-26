using API_DatabaseMarket.DTOs.Orders;

namespace API_DatabaseMarket.Services
{
    public interface IPricingService
    {
        decimal Calculate(CreateOrderItemDto dto);
    }
}
