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
                Config = CreateJson("{\"test\":true}"),
                CreatedAt = DateTime.UtcNow
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
                Config = CreateJson("{\"updated\":true}"),
                CreatedAt = created.CreatedAt
            };

            
            var result = await service.UpdateAsync(created.Id, updatedEntity);

            
            Assert.True(result);
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
