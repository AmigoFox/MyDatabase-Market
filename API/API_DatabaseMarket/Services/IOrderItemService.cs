using API_DatabaseMarket.DTOs;
using API_DatabaseMarket.Models;

namespace API_DatabaseMarket.Services
{
    public interface IOrderItemService
    {
        IEnumerable<(int Id, OrderItemDto Data)> GetAll();
        (int Id, OrderItemDto Data)? GetById(int id);
        (int Id, OrderItemDto Data) Create(OrderItemDto dto);
        bool Update(int id, OrderItemDto dto);
        bool Delete(int id);
    }
}
