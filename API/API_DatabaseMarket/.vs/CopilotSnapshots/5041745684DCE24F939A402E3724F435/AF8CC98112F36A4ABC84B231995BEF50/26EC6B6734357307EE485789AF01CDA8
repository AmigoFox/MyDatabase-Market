using API_DatabaseMarket.Data;
using API_DatabaseMarket.DTOs.Orders;
using API_DatabaseMarket.DTOs;
using API_DatabaseMarket.Models;
using API_DatabaseMarket.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace API_DatabaseMarket.Controllers
{
    [ApiController]
    [Route("api/v1/orders")]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IPricingService _pricingService;
        private readonly IExchangeRateService _exchangeService;
        private readonly IOrderService _orderService;
        public OrdersController(AppDbContext db, IPricingService pricingService, IExchangeRateService exchangeService, IOrderService orderService)
        {
            _db = db;
            _pricingService = pricingService;
            _exchangeService = exchangeService;
            _orderService = orderService;
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
                OrderItems = new List<OrderItem>(),
                OrderName = string.IsNullOrWhiteSpace(request.OrderName)
                ? $"{request.Items.First().DatabaseType} ({request.Items.First().SizeGB}GB)"
                : request.OrderName,

                PaymentDueDate = DateTime.UtcNow.AddDays(31)
            };

            decimal totalAmount = 0m;

            foreach (var itemDto in request.Items)
            {

                decimal calculatedPrice = _pricingService.Calculate(itemDto);
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
        public async Task<ActionResult<IEnumerable<OrderResponse>>> GetMyOrders([FromQuery] string? currencies)
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;

            if (userIdClaim == null)
                return Unauthorized();

            int userId = int.Parse(userIdClaim);



            var requestedCurrencies = string.IsNullOrWhiteSpace(currencies)
                ? new List<string>()
                : currencies.Split(',', StringSplitOptions.RemoveEmptyEntries)
                            .Select(c => c.Trim().ToUpper())
                            .ToList();

            Dictionary<string, decimal> rates = new();

            if (requestedCurrencies.Any())
            {
                rates = await _db.ExchangeRates
                    .Where(x => requestedCurrencies.Contains(x.CurrencyCode))
                    .ToDictionaryAsync(x => x.CurrencyCode, x => x.RateToRub);
            }

            var orders = await _db.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderItems)
                    .ThenInclude(i => i.Countries)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();


            var result = new List<OrderResponse>();

            foreach (var o in orders)
            {
                var response = new OrderResponse
                {
                    Id = o.Id,
                    TotalAmount = o.TotalAmount,

                    Status = (o.PaymentDueDate.HasValue &&
                              o.PaymentDueDate < DateTime.UtcNow &&
                              o.Status != "paid")
                        ? "expired"
                        : o.Status,

                    CreatedAt = o.CreatedAt,
                    OrderName = o.OrderName,
                    PaymentDueDate = o.PaymentDueDate
                };

                foreach (var currency in requestedCurrencies)
                {
                    if (rates.TryGetValue(currency, out var rate))
                    {
                        response.Prices[currency] =
                            Math.Round(o.TotalAmount / rate, 2);
                    }
                }

                foreach (var i in o.OrderItems)
                {
                    var itemResponse = new OrderItemResponse
                    {
                        Id = i.Id,
                        DatabaseType = i.DatabaseType,
                        SizeGB = i.SizeGB,
                        Iops = i.Iops,
                        StorageType = i.StorageType,
                        Scalability = i.Scalability,
                        FinalPriceRub = i.FinalPriceRub,
                        Countries = i.Countries.Select(c => c.CountryCode).ToList(),
                        Config = i.Config,
                        CreatedAt = i.CreatedAt,
                        OrderName = o.OrderName
                    };

                    foreach (var currency in requestedCurrencies)
                    {
                        if (rates.TryGetValue(currency, out var rate))
                        {
                            itemResponse.Prices[currency] =
                                Math.Round(i.FinalPriceRub / rate, 2);
                        }
                    }

                    response.Items.Add(itemResponse);
                }

                result.Add(response);
            }
            return Ok(result);
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

                        Status = (o.PaymentDueDate.HasValue &&
                                  o.PaymentDueDate < DateTime.UtcNow &&
                                  o.Status != "paid")
                            ? "expired"
                            : o.Status,

                        CreatedAt = o.CreatedAt,
                        OrderName = o.OrderName,
                        PaymentDueDate = o.PaymentDueDate,

                        Items = o.OrderItems.Select(i => new OrderItemResponse
                        {
                            Id = i.Id,
                            DatabaseType = i.DatabaseType,
                            SizeGB = i.SizeGB,
                            Iops = i.Iops,
                            StorageType = i.StorageType,
                            Scalability = i.Scalability,
                            FinalPriceRub = i.FinalPriceRub,
                            OrderName = o.OrderName,

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
    
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;

            if (userIdClaim == null)
                return Unauthorized();

            int userId = int.Parse(userIdClaim);

            var deleted = await _orderService.DeleteOrderAsync(id, userId);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}