using API_DatabaseMarket.Data;
using API_DatabaseMarket.Models;
using API_DatabaseMarket.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace API_DatabaseMarket.Tests
{
    public class OrderItemServiceTests
    {
        private AppDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        private static OrderItem CreateEntity()
        {
            return new OrderItem
            {
                OrderId = 1,
                DatabaseType = "PostgreSQL",
                SizeGB = 50,
                Iops = "Средняя (1000)",
                StorageType = "SSD",
                Scalability = "Replication",
                FinalPriceRub = 5000m,
                Config = CreateJson("{\"test\":true}"),
                CreatedAt = DateTime.UtcNow,
                Countries = new List<OrderItemCountry>
                {
                    new OrderItemCountry
                    {
                        CountryCode = "RU"
                    }
                }
            };
        }

        [Fact]
        public async Task Create_AddsNewItem()
        {
            var context = CreateDbContext();
            var service = new OrderItemService(context);
            var entity = CreateEntity();

            var result = await service.CreateAsync(entity);

            Assert.NotNull(result);
            Assert.True(result.Id > 0);
            Assert.Equal(1, await context.OrderItems.CountAsync());
        }

        [Fact]
        public async Task GetAll_ReturnsItems_WhenItemsExist()
        {
            var context = CreateDbContext();
            var service = new OrderItemService(context);

            await service.CreateAsync(CreateEntity());

            var items = await service.GetAllAsync();

            Assert.NotEmpty(items);
        }

        [Fact]
        public async Task GetById_ReturnsItem_WhenItemExists()
        {
            var context = CreateDbContext();
            var service = new OrderItemService(context);

            var created = await service.CreateAsync(CreateEntity());

            var result = await service.GetByIdAsync(created.Id);

            Assert.NotNull(result);
            Assert.Equal(created.Id, result!.Id);
            Assert.Equal("PostgreSQL", result.DatabaseType);
        }

        [Fact]
        public async Task Update_ReturnsTrue_WhenItemExists()
        {
            var context = CreateDbContext();
            var service = new OrderItemService(context);

            var created = await service.CreateAsync(CreateEntity());

            var updatedEntity = new OrderItem
            {
                OrderId = 1,
                DatabaseType = "MySQL",
                SizeGB = 100,
                Iops = "Высокая (5000)",
                StorageType = "NVMe",
                Scalability = "Autoscaling",
                FinalPriceRub = 10000m,
                Config = CreateJson("{\"updated\":true}"),
                CreatedAt = created.CreatedAt,
                Countries = new List<OrderItemCountry>
                {
                    new OrderItemCountry
                    {
                        CountryCode = "US"
                    }
                }
            };

            var result = await service.UpdateAsync(created.Id, updatedEntity);

            Assert.True(result);

            var updated = await service.GetByIdAsync(created.Id);
            Assert.Equal("MySQL", updated!.DatabaseType);
            Assert.Equal(10000m, updated.FinalPriceRub);
        }

        [Fact]
        public async Task Update_ReturnsFalse_WhenItemDoesNotExist()
        {
            var context = CreateDbContext();
            var service = new OrderItemService(context);

            var result = await service.UpdateAsync(999, CreateEntity());

            Assert.False(result);
        }

        [Fact]
        public async Task Delete_ReturnsTrue_WhenItemExists()
        {
            var context = CreateDbContext();
            var service = new OrderItemService(context);

            var created = await service.CreateAsync(CreateEntity());

            var result = await service.DeleteAsync(created.Id);

            Assert.True(result);
            Assert.Empty(await service.GetAllAsync());
        }

        [Fact]
        public async Task Delete_ReturnsFalse_WhenItemDoesNotExist()
        {
            var context = CreateDbContext();
            var service = new OrderItemService(context);

            var result = await service.DeleteAsync(999);

            Assert.False(result);
        }

        private static JsonElement CreateJson(string json)
        {
            using var doc = JsonDocument.Parse(json);
            return doc.RootElement.Clone();
        }
    }
}