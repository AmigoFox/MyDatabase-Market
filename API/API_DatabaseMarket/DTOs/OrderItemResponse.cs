namespace API_DatabaseMarket.DTOs
{
    public class OrderItemResponse
    {
        public int Id { get; set; }

        public string DatabaseType { get; set; } = "";

        public int SizeGB { get; set; }

        public string Iops { get; set; } = "";

        public string StorageType { get; set; } = "";

        public string Scalability { get; set; } = "";

        public decimal FinalPriceRub { get; set; }

        public List<string> Countries { get; set; } = new();
    }
}