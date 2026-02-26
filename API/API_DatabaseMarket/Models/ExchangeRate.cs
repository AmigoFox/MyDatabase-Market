using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_DatabaseMarket.Models
{
        [Table("exchange_rates")]
        public class ExchangeRate
        {
            [Key]
            public int Id { get; set; }

            [Required]
            [MaxLength(10)]
            public string CurrencyCode { get; set; } = default!;

            [Column(TypeName = "numeric(18,6)")]
            public decimal RateToRub { get; set; }

            public DateTime UpdatedAt { get; set; }
        }
}
