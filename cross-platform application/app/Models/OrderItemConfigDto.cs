using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.Models
{
    public class OrderItemConfigDto
    {
        public decimal Price { get; set; }
        public string Currency { get; set; } = "";
        public int Quantity { get; set; }
        public int ProductId { get; set; }
        public string Warehouse { get; set; } = "";

        // optional поле (оно не всегда есть)
        public string? CreatedBy { get; set; }
    }
}
