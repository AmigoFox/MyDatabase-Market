using API_DatabaseMarket;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

[ApiController]
[Route("api/[controller]")]
public class OrderItemsController : ControllerBase
{
    private static readonly List<(int Id, OrderItemDto Data)> _items = new();
    private static int _idCounter = 1;

    // GET: api/orderitems
    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_items);
    }

    // GET: api/orderitems/{id}
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var item = _items.FirstOrDefault(x => x.Id == id);
        if (item.Id == 0)
            return NotFound();

        return Ok(item);
    }

    // POST: api/orderitems
    [HttpPost]
    public IActionResult Create([FromBody] OrderItemDto dto)
    {
        var newItem = (_idCounter++, dto);
        _items.Add(newItem);
        return CreatedAtAction(nameof(GetById), new { id = newItem.Item1 }, newItem);
    }

    // PUT: api/orderitems/{id}
    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] OrderItemDto dto)
    {
        var index = _items.FindIndex(x => x.Id == id);
        if (index == -1)
            return NotFound();

        _items[index] = (id, dto);
        return Ok(_items[index]);
    }

    // DELETE: api/orderitems/{id}
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var index = _items.FindIndex(x => x.Id == id);
        if (index == -1)
            return NotFound();

        _items.RemoveAt(index);
        return NoContent();
    }
}
