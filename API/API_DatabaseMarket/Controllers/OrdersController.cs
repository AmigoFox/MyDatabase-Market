using API_DatabaseMarket.Data;
using API_DatabaseMarket.DTOs.Orders;
using API_DatabaseMarket.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_DatabaseMarket.Controllers
{
    [ApiController]
    [Route("api/v1/orders")]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly AppDbContext _db;

        public OrdersController(AppDbContext db)
        {
            _db = db;
        }

        // ============================
        // POST /api/v1/orders
        // ============================
        [HttpPost]
        public async Task<ActionResult<int>> CreateOrder(CreateOrderRequest request)
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (userIdClaim == null)
                return Unauthorized();

            int userId = int.Parse(userIdClaim);

            if (request.Items == null || !request.Items.Any())
                return BadRequest("Order must contain at least one item.");

            var order = new Order
            {
                UserId = userId,
                Status = "created",
                CreatedAt = DateTime.UtcNow,
                OrderItems = new List<OrderItem>()
            };

            decimal totalAmount = 0m;

            foreach (var itemDto in request.Items)
            {
                // 🔥 Пока временный расчёт (заглушка)
                decimal calculatedPrice = itemDto.SizeGB * 10m;

                var orderItem = new OrderItem
                {
                    DatabaseType = itemDto.DatabaseType,
                    SizeGB = itemDto.SizeGB,
                    Iops = itemDto.Iops,
                    StorageType = itemDto.StorageType,
                    Scalability = itemDto.Scalability,
                    FinalPriceRub = calculatedPrice,
                    Config = itemDto.Config ?? default,
                    CreatedAt = DateTime.UtcNow,
                    Countries = itemDto.Countries
                        .Select(c => new OrderItemCountry
                        {
                            CountryCode = c
                        }).ToList()
                };

                totalAmount += calculatedPrice;
                order.OrderItems.Add(orderItem);
            }

            order.TotalAmount = totalAmount;

            _db.Orders.Add(order);
            await _db.SaveChangesAsync();

            return Ok(order.Id);
        }

        // ============================
        // GET /api/v1/orders
        // ============================
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderResponse>>> GetMyOrders()
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (userIdClaim == null)
                return Unauthorized();

            int userId = int.Parse(userIdClaim);

            var orders = await _db.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderItems)
                    .ThenInclude(i => i.Countries)
                .OrderByDescending(o => o.CreatedAt)
                .Select(o => new OrderResponse
                {
                    Id = o.Id,
                    TotalAmount = o.TotalAmount,
                    Status = o.Status,
                    CreatedAt = o.CreatedAt,
                    Items = o.OrderItems.Select(i => new OrderItemResponse
                    {
                        Id = i.Id,
                        DatabaseType = i.DatabaseType,
                        SizeGB = i.SizeGB,
                        Iops = i.Iops,
                        StorageType = i.StorageType,
                        Scalability = i.Scalability,
                        FinalPriceRub = i.FinalPriceRub,
                        Countries = i.Countries
                            .Select(c => c.CountryCode)
                            .ToList(),
                        Config = i.Config,
                        CreatedAt = i.CreatedAt
                    }).ToList()
                })
                .ToListAsync();

            return Ok(orders);
        }

        // ============================
        // GET /api/v1/orders/{id}
        // ============================
        [HttpGet("{id:int}")]
        public async Task<ActionResult<OrderResponse>> GetOrderById(int id)
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (userIdClaim == null)
                return Unauthorized();

            int userId = int.Parse(userIdClaim);

            var order = await _db.Orders
                .Where(o => o.Id == id && o.UserId == userId)
                .Include(o => o.OrderItems)
                    .ThenInclude(i => i.Countries)
                .Select(o => new OrderResponse
                {
                    Id = o.Id,
                    TotalAmount = o.TotalAmount,
                    Status = o.Status,
                    CreatedAt = o.CreatedAt,
                    Items = o.OrderItems.Select(i => new OrderItemResponse
                    {
                        Id = i.Id,
                        DatabaseType = i.DatabaseType,
                        SizeGB = i.SizeGB,
                        Iops = i.Iops,
                        StorageType = i.StorageType,
                        Scalability = i.Scalability,
                        FinalPriceRub = i.FinalPriceRub,
                        Countries = i.Countries
                            .Select(c => c.CountryCode)
                            .ToList(),
                        Config = i.Config,
                        CreatedAt = i.CreatedAt
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (order == null)
                return NotFound();

            return Ok(order);
        }
    }
}