using API_DatabaseMarket.Data;
using API_DatabaseMarket.Models;
using Microsoft.EntityFrameworkCore;


namespace API_DatabaseMarket.Services
{

    public class OrderItemService : IOrderItemService
    {
        private readonly AppDbContext _context;

        public OrderItemService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OrderItem>> GetAllAsync()
        {
            return await _context.OrderItems
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<OrderItem?> GetByIdAsync(int id)
        {
            return await _context.OrderItems
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<OrderItem> CreateAsync(OrderItem item)
        {
            item.CreatedAt = DateTime.UtcNow;

            _context.OrderItems.Add(item);
            await _context.SaveChangesAsync();

            return item;
        }

        public async Task<bool> UpdateAsync(int id, OrderItem item)
        {
            var existing = await _context.OrderItems.FindAsync(id);
            if (existing == null)
                return false;

            existing.Config = item.Config;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var item = await _context.OrderItems.FindAsync(id);
            if (item == null)
                return false;

            _context.OrderItems.Remove(item);
            await _context.SaveChangesAsync();

            return true;
        }
    }


}
