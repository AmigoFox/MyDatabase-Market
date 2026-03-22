namespace API_DatabaseMarket.DTOs.Payments
{
    public class PaymentResultDto
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = default!;
        public string Status { get; set; } = default!;
        public string TransactionId { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
    }
}
