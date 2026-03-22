using API_DatabaseMarket.Data;
using Microsoft.EntityFrameworkCore;
using API_DatabaseMarket.Services;
using API_DatabaseMarket.Controllers;
using API_DatabaseMarket.Models;

namespace API_DatabaseMarket.Services
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _db;

        public OrderService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<bool> DeleteOrderAsync(int id)        
        {

            var order = await _db.Orders.FindAsync(id);
            if (order == null)
                return false;

            var orderItems = await _db.OrderItems
                .Where(i => i.OrderId == id)
                .ToListAsync();

            _db.OrderItems.RemoveRange(orderItems);

            if (order == null)
                return false;

            _db.Orders.Remove(order);

            await _db.SaveChangesAsync();

            return true;
        }
    }
}
