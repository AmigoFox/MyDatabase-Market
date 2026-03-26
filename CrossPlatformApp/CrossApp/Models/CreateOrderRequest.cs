using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrossApp.Models;
using System.Text.Json.Serialization;
using CrossApp.Services.Api;
using CrossApp.Services;


namespace CrossApp.Models;
public class CreateOrderRequest
{
    public List<CreateOrderItemDto> Items { get; set; } = new();

    public string? OrderName { get; set; }
}
public class CreateOrderItemDto
{
    public string DatabaseType { get; set; } = "";
    public int SizeGB { get; set; }
    public string Iops { get; set; } = "";
    public string StorageType { get; set; } = "";
    public string Scalability { get; set; } = "";

    public List<string> Countries { get; set; } = new();

    public string OrderName { get; set; } = string.Empty;

    public OrderItemConfigDto Config { get; set; } = new();
}