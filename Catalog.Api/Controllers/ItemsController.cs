using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Catalog.Api.Repositories;
using Catalog.Api.Models;
using Catalog.Api.Dtos;

namespace Catalog.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemsController : ControllerBase
    {
        // explicite dependency is bad 
        private readonly IItemsRepository repository; 

        public ItemsController(IItemsRepository repository) 
        {
            this.repository = repository;
        }

        // GET /items 
        [HttpGet]
        public async Task<IEnumerable<ItemDto>> GetItemsAsync(string name = null) 
        {
            var items = (await repository.GetItemsAsync())
                    .Select(item => item.AsDto());

            if(!string.IsNullOrWhiteSpace(name)) {
                items = items.Where(
                    item => item.Name.Contains(name, StringComparison.OrdinalIgnoreCase)
                );
            }

            return items;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetItemAsync(Guid id) 
        {
            var item = await repository.GetItemAsync(id);
            
            if(item is null) {
                return NotFound();
            }

            return item.AsDto();
        }

        // POST /items
        [HttpPost]
        public async Task <ActionResult<ItemDto>> CreateItemAsync(CreateItemDto itemDto) 
        {
            Item item = new () 
            {
                Id = Guid.NewGuid(),
                Name = itemDto.Name,
                Description = itemDto.Description,
                Price = itemDto.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };

            await repository.CreateItemAsync(item);

            return CreatedAtAction(nameof(GetItemAsync), new { id = item.Id }, item.AsDto());
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateItemAsync(Guid id, UpdateItemDto itemDto) 
        {
            var existingItem = await repository.GetItemAsync(id);

            if(existingItem is null) {
                return NotFound();
            }

            existingItem.Name = itemDto.Name;
            existingItem.Price = itemDto.Price;

            await repository.UpdateItemAsync(existingItem);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteItemAsync(Guid id)
        {
            var existingItem = await repository.GetItemAsync(id);

            if(existingItem is null) {
                return NotFound();
            }

            await repository.DeleteItemAsync(id);

            return NoContent();
        }

    }
}