using API_DatabaseMarket.DTOs;
using API_DatabaseMarket.Models;


namespace API_DatabaseMarket.Services
{
    public class OrderItemService : IOrderItemService
    {
        private readonly List<(int Id, OrderItemDto Data)> _items = new();
        private int _idCounter = 1;

        public IEnumerable<(int Id, OrderItemDto Data)> GetAll()
            => _items;

        public (int Id, OrderItemDto Data)? GetById(int id)
        {
            var item = _items.FirstOrDefault(x => x.Id == id);
            return item.Id == 0 ? null : item;
        }

        public (int Id, OrderItemDto Data) Create(OrderItemDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var newItem = (_idCounter++, dto);
            _items.Add(newItem);
            return newItem;
        }

        public bool Update(int id, OrderItemDto dto)
        {
            var index = _items.FindIndex(x => x.Id == id);
            if (index == -1)
                return false;

            _items[index] = (id, dto);
            return true;
        }

        public bool Delete(int id)
        {
            var index = _items.FindIndex(x => x.Id == id);
            if (index == -1)
                return false;

            _items.RemoveAt(index);
            return true;
        }
    }
}
