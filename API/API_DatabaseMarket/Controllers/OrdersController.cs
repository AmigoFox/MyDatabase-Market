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

            var order = new Order
            {
                UserId = userId,
                TotalAmount = request.TotalAmount,
                Status = "created",
                CreatedAt = DateTime.UtcNow,
                OrderItems = request.Items.Select(i => new OrderItem
                {
                    Config = i.Config, // ✅ JsonElement → JsonElement
                    CreatedAt = DateTime.UtcNow
                }).ToList()
            };

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
                        Config = i.Config, // ✅ уже JsonElement
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
                .Select(o => new OrderResponse
                {
                    Id = o.Id,
                    TotalAmount = o.TotalAmount,
                    Status = o.Status,
                    CreatedAt = o.CreatedAt,
                    Items = o.OrderItems.Select(i => new OrderItemResponse
                    {
                        Id = i.Id,
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
