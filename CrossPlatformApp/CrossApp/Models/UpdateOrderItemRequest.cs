using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossApp.Models
{
    public class UpdateOrderItemRequest
    {
        public int OrderId { get; set; }

        public string DatabaseType { get; set; } = "";
        public int SizeGB { get; set; }
        public string Iops { get; set; } = "";
        public string StorageType { get; set; } = "";
        public string Scalability { get; set; } = "";
        public decimal FinalPriceRub { get; set; }

        public List<string>? Countries { get; set; }

        public OrderItemConfigDto Config { get; set; } = new();
    }
}
