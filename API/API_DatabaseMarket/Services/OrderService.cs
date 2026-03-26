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
        public async Task<bool> DeleteOrderAsync(int id, int userId)
        {
            var order = await _db.Orders
                .FirstOrDefaultAsync(o => o.Id == id && o.UserId == userId);

            if (order == null)
                return false;

            _db.Orders.Remove(order);

            await _db.SaveChangesAsync();

            return true;
        }
    }
}
