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
                .Include(x => x.Countries)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<OrderItem?> GetByIdAsync(int id)
        {
            return await _context.OrderItems
                .Include(x => x.Countries)
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
    var existing = await _context.OrderItems
        .Include(x => x.Countries)
        .FirstOrDefaultAsync(x => x.Id == id);

    if (existing == null)
        return false;

            // Если вы гарантируете, что контроллер передаёт все нужные поля — можно присваивать напрямую.
            // ???? ?????????? ???????? ????????? ?????? — ??????????? ?????? ?? ????, ??????? ????????????? ????????
            if (!string.IsNullOrEmpty(item.DatabaseType))
                existing.DatabaseType = item.DatabaseType;

            if (item.SizeGB != 0)
                existing.SizeGB = item.SizeGB;

            if (!string.IsNullOrEmpty(item.Iops))
                existing.Iops = item.Iops;

            if (!string.IsNullOrEmpty(item.StorageType))
                existing.StorageType = item.StorageType;

            if (!string.IsNullOrEmpty(item.Scalability))
                existing.Scalability = item.Scalability;

            if (item.FinalPriceRub != 0m)
                existing.FinalPriceRub = item.FinalPriceRub;

            // ????????? Config ?????? ???? ??? ??????? ???????? JSON-??????
            if (item.Config.ValueKind != System.Text.Json.JsonValueKind.Undefined)
                existing.Config = item.Config;

            // Обновляем страны только если они переданы в item (иначе оставляем текущие)
            if (item.Countries != null)
    {
        existing.Countries.Clear();
        foreach (var country in item.Countries)
        {
            existing.Countries.Add(new OrderItemCountry
            {
                CountryCode = country.CountryCode
            });
        }
    }

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
