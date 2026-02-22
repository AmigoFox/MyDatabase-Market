using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.Models
{
    public class OrderItemDto
    {
        public int Id { get; set; }
        public OrderItemConfigDto Config { get; set; } = new();
        public DateTime CreatedAt { get; set; }
    }
}
