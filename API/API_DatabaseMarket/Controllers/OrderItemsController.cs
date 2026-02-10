using API_DatabaseMarket.DTOs;
using API_DatabaseMarket.Models;
using API_DatabaseMarket.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace API_DatabaseMarket.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class OrderItemsController : ControllerBase
    {
        private readonly IOrderItemService _service;

        public OrderItemsController(IOrderItemService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _service.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {

            var item = await _service.GetByIdAsync(id);
            if (item == null)
                return NotFound();

            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OrderItemDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var configError = ValidateConfig(dto);
            if (configError != null)
                return configError;

            var entity = new OrderItem
            {
                OrderId = dto.OrderId,
                Config = dto.Config,
                CreatedAt = DateTime.UtcNow
            };


            var created = await _service.CreateAsync(entity);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] OrderItemDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var configError = ValidateConfig(dto);
            if (configError != null)
                return configError;

            var entity = new OrderItem
            {

                OrderId = dto.OrderId,
                Config = dto.Config,
                CreatedAt = DateTime.UtcNow
            };

            var updated =  await _service.UpdateAsync(id, entity);

            if (!updated)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }


        private IActionResult? ValidateConfig(OrderItemDto dto)
        {
            if (dto.Config.ValueKind != JsonValueKind.Object)
                return BadRequest("Config must be a JSON object");

            return null;
        }

    }
}
