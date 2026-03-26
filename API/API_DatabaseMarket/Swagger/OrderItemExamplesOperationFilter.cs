using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace API_DatabaseMarket.Swagger
{
    public class OrderItemExamplesOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.RequestBody == null || context.ApiDescription.HttpMethod == null)
                return;

            // Only modify PUT /OrderItems/{id}
            var relativePath = context.ApiDescription.RelativePath?.ToLowerInvariant() ?? string.Empty;
            var httpMethod = context.ApiDescription.HttpMethod?.ToUpperInvariant();

            if (httpMethod == "PUT" && relativePath.Contains("orderitems"))
            {
                var mediaType = "application/json";
                if (operation.RequestBody.Content.TryGetValue(mediaType, out var content))
                {
                    var example = new OpenApiObject
                    {
                        ["databaseType"] = new OpenApiString("MySQL"),
                        ["sizeGB"] = new OpenApiInteger(10111),
                        ["iops"] = new OpenApiString("Низкая (100)"),
                        ["storageType"] = new OpenApiString("SSD"),
                        ["scalability"] = new OpenApiString("None"),
                        ["finalPriceRub"] = new OpenApiDouble(1225.32),
                        ["countries"] = new OpenApiArray { new OpenApiString("RU") },
                        ["config"] = new OpenApiObject
                        {
                            ["backup"] = new OpenApiBoolean(false),
                            ["sharding"] = new OpenApiBoolean(false),
                            ["replicaSet"] = new OpenApiBoolean(false)
                        },
                        ["orderName"] = new OpenApiString("Новый заказ")
                    };

                    content.Example = example;
                }
            }
        }
    }
}
