using Xunit;
using API_DatabaseMarket.Services;
using API_DatabaseMarket.DTOs;

namespace API_DatabaseMarket.Tests
{
    public class OrderItemServiceTests
    {
        [Fact]
        public void Create_AddsNewItem()
        {
            var service = new OrderItemService();
            var dto = new OrderItemDto();

            var result = service.Create(dto);

            Assert.NotNull(result);
            Assert.True(result.Id > 0);
        }


        [Fact]
        public void GetAll_ReturnsItems_WhenItemsExist()
        {
            var service = new OrderItemService();
            var dto = new OrderItemDto();

            service.Create(dto);

            var items = service.GetAll();

            Assert.NotEmpty(items);
        }


        [Fact]
        public void GetById_ReturnsItem_WhenItemExists()
        {
            var service = new OrderItemService();
            var dto = new OrderItemDto();

            var created = service.Create(dto);

            var result = service.GetById(created.Id);

            Assert.NotNull(result);
            Assert.Equal(created.Id, result.Value.Id);
        }

        [Fact]
        public void Update_ReturnsTrue_WhenItemExists()
        {
            var service = new OrderItemService();
            var dto = new OrderItemDto();
            var created = service.Create(dto);

            var newDto = new OrderItemDto();

            var result = service.Update(created.Id, newDto);

            Assert.True(result);
        }


        [Fact]
        public void Update_ReturnsFalse_WhenItemDoesNotExist()
        {
            var service = new OrderItemService();
            var dto = new OrderItemDto();

            var result = service.Update(123, dto);

            Assert.False(result);
        }



        [Fact]
        public void Delete_ReturnsTrue_WhenItemExists()
        {
            var service = new OrderItemService();
            var dto = new OrderItemDto();
            var created = service.Create(dto);

            var result = service.Delete(created.Id);

            Assert.True(result);
        }


        [Fact]
        public void Delete_ReturnsFalse_WhenItemDoesNotExist()
        {
            var service = new OrderItemService();

            var result = service.Delete(999);

            Assert.False(result);
        }
    }
}
