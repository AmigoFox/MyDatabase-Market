using API_DatabaseMarket.Data;
using API_DatabaseMarket.Models;
using API_DatabaseMarket.DTOs.Payments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace API_DatabaseMarket.Controllers
{
    [ApiController]
    [Route("api/v1/payments")]
    [Authorize]
    public class PaymentsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PaymentsController(AppDbContext context)
        {
            _context = context;
        }

        // POST /api/v1/payments
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePaymentRequest request)
        {
            Debug.WriteLine("CREATE PAYMENT HIT");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (userIdClaim == null)
                return Unauthorized();

            if (request == null)
                return BadRequest("Invalid request");

            int userId = int.Parse(userIdClaim);

            Debug.WriteLine($"UserId: {userId}");
            Debug.WriteLine($"OrderId: {request.OrderId}");

            // Найти заказ, принадлежащий текущему пользователю
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.Id == request.OrderId && o.UserId == userId);

            if (order == null)
                return BadRequest("Order not found");

            Debug.WriteLine($"Order owner UserId: {order.UserId}");

            // Проверка на уже выполнённую оплату
            var exists = await _context.Payments
                .AnyAsync(p => p.OrderId == request.OrderId && p.Status == "Completed");

            if (exists)
                return BadRequest("Order already paid");

            // Создаём запись платежа (сумма берётся с заказа)
            var payment = new Payment
            {
                OrderId = request.OrderId,
                Amount = order.TotalAmount,
                PaymentMethod = string.IsNullOrWhiteSpace(request.PaymentMethod) ? "Card" : request.PaymentMethod,
                Status = "Completed",
                TransactionId = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.UtcNow
            };

            _context.Payments.Add(payment);

            // Обновляем статус заказа
            order.Status = "paid";

            await _context.SaveChangesAsync();

            // Маппим в DTO, чтобы избежать сериализационных циклов
            var result = new PaymentResponse
            {
                Id = payment.Id,
                OrderId = payment.OrderId,
                Amount = payment.Amount,
                PaymentMethod = payment.PaymentMethod ?? "Card",
                Status = payment.Status,
                CreatedAt = payment.CreatedAt
            };

            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        // GET /api/v1/payments
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var payments = await _context.Payments
                .AsNoTracking()
                .OrderByDescending(x => x.CreatedAt)
                .Select(p => new PaymentResponse
                {
                    Id = p.Id,
                    OrderId = p.OrderId,
                    Amount = p.Amount,
                    PaymentMethod = p.PaymentMethod,
                    Status = p.Status,
                    CreatedAt = p.CreatedAt
                })
                .ToListAsync();

            return Ok(payments);
        }

        // GET /api/v1/payments/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var p = await _context.Payments
                .AsNoTracking()
                .Where(x => x.Id == id)
                .Select(x => new PaymentResponse
                {
                    Id = x.Id,
                    OrderId = x.OrderId,
                    Amount = x.Amount,
                    PaymentMethod = x.PaymentMethod,
                    Status = x.Status,
                    CreatedAt = x.CreatedAt
                })
                .FirstOrDefaultAsync();

            if (p == null)
                return NotFound();

            return Ok(p);
        }
    }
}