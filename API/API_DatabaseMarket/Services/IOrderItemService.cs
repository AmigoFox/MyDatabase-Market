using API_DatabaseMarket.DTOs;
using API_DatabaseMarket.Models;

namespace API_DatabaseMarket.Services
{
    public interface IOrderItemService
    {
        Task<IEnumerable<OrderItem>> GetAllAsync();
        Task<OrderItem?> GetByIdAsync(int id);
        Task<OrderItem> CreateAsync(OrderItem item);
        Task<bool> UpdateAsync(int id, OrderItem item, string? orderName = null);
        Task<bool> DeleteAsync(int id);
    }

}
