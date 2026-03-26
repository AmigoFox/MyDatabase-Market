using API_DatabaseMarket.Data;
using API_DatabaseMarket.Models;
using API_DatabaseMarket.DTOs.Payments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (userIdClaim == null)
                return Unauthorized();

            int userId = int.Parse(userIdClaim);

            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.Id == request.OrderId && o.UserId == userId);

            if (order == null)
                return BadRequest("Order not found");

            // Проверка дедлайна оплаты
            if (order.PaymentDueDate.HasValue && order.PaymentDueDate < DateTime.UtcNow)
            {
                order.Status = "expired";
                await _context.SaveChangesAsync();

                return BadRequest("Payment deadline expired");
            }

            // Уже оплачен
            if (order.Status == "paid")
                return BadRequest("Order already paid");

            // Нормализация метода оплаты
            var paymentMethod = string.IsNullOrWhiteSpace(request.PaymentMethod)
                ? "card"
                : request.PaymentMethod.ToLower();

            // Создание платежа
            var payment = new Payment
            {
                OrderId = request.OrderId,
                Amount = order.TotalAmount,
                PaymentMethod = paymentMethod,
                Status = "paid",
                TransactionId = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.UtcNow
            };

            _context.Payments.Add(payment);

            // Обновляем заказ
            order.Status = "paid";

            await _context.SaveChangesAsync();

            var result = new PaymentResponse
            {
                Id = payment.Id,
                OrderId = payment.OrderId,
                Amount = payment.Amount,
                PaymentMethod = paymentMethod,
                Status = payment.Status,
                CreatedAt = payment.CreatedAt
            };

            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        // GET /api/v1/payments
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = int.Parse(User.FindFirst("UserId")!.Value);

            var payments = await _context.Payments
                .Where(p => p.Order.UserId == userId)
                .AsNoTracking()
                .OrderByDescending(x => x.CreatedAt)
                .Select(p => new PaymentResponse
                {
                    Id = p.Id,
                    OrderId = p.OrderId,
                    Amount = p.Amount,
                    PaymentMethod = p.PaymentMethod ?? "card",
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
            var userId = int.Parse(User.FindFirst("UserId")!.Value);

            var payment = await _context.Payments
                .AsNoTracking()
                .Where(x => x.Id == id && x.Order.UserId == userId)
                .Select(x => new PaymentResponse
                {
                    Id = x.Id,
                    OrderId = x.OrderId,
                    Amount = x.Amount,
                    PaymentMethod = x.PaymentMethod ?? "card",
                    Status = x.Status,
                    CreatedAt = x.CreatedAt
                })
                .FirstOrDefaultAsync();

            if (payment == null)
                return NotFound();

            return Ok(payment);
        }
    }
}