using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossApp.Models
{
    public class OrderItemDto
    {
        public int Id { get; set; }
        public OrderItemConfigDto Config { get; set; } = new();
        public DateTime CreatedAt { get; set; }
        public string DatabaseType { get; set; } = "";
        public int SizeGB { get; set; }
        public string Iops { get; set; } = "";
        public decimal FinalPriceRub { get; set; }
    }
}
