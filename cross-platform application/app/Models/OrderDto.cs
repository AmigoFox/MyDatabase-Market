using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.Models
{
    public class OrderDto
    {
        public int Id { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = "";
        public DateTime CreatedAt { get; set; }

        public List<OrderItemDto> Items { get; set; } = new();

        public int ItemsCount => Items?.Count ?? 0;

        public string FormattedDate => CreatedAt.ToString("dd.MM.yyyy HH:mm");

        public string FormattedAmount => $"{TotalAmount:0.00} €";
    }
}
