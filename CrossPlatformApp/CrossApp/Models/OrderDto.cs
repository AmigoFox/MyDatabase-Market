using CrossApp.Models;

namespace CrossApp.Models;

public class OrderDto
{
    public int Id { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = "";
    public DateTime CreatedAt { get; set; }

    public List<OrderItemDto> Items { get; set; } = new();

    public int ItemsCount => Items?.Count ?? 0;

    public string FormattedDate => CreatedAt.ToString("dd.MM.yyyy HH:mm");

    public string DatabaseType => Items?.FirstOrDefault()?.DatabaseType ?? "";

    public int SizeGB => Items?.FirstOrDefault()?.SizeGB ?? 0;

    public string Iops => Items?.FirstOrDefault()?.Iops ?? "";

    public decimal FinalPriceRub => Items?.FirstOrDefault()?.FinalPriceRub ?? 0;

    public string? OrderName { get; set; }


    public DateTime? PaymentDueDate { get; set; }


    public bool ShowPaymentDate => Status == "paid";

    public string StatusText =>
        Status == "paid"
            ? "Оплачен"
            : "Оплатить";

    public string StatusColor =>
        Status == "paid"
            ? "#4CAF50"
            : "#FB8C00";




    public string Countries =>
        Items?.FirstOrDefault()?.Countries != null
            ? string.Join(", ", Items.First().Countries)
            : "";

    public string FormattedAmount => $"{TotalAmount:0.00} €";
}