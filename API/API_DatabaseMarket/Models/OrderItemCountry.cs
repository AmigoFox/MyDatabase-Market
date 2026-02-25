namespace API_DatabaseMarket.Models
{
    public class OrderItemCountry
    {
        public int Id { get; set; }
        public int OrderItemId { get; set; }
        public string CountryCode { get; set; } = default!;
        public OrderItem OrderItem { get; set; } = default!;
    }
}
