using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CatalogApi.Repositories;
using CatalogApi.Models;
using CatalogApi.Dtos;

namespace CatalogApi.Controllers
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
        public IEnumerable<ItemDto> GetItems() 
        {
            return repository.GetItemsAsync().Select(item => item.AsDto());
        }

        [HttpGet("{id}")]
        public ActionResult<ItemDto> GetItem(Guid id) 
        {
            var item = repository.GetItemAsync(id);
            
            if(item is null) {
                return NotFound();
            }

            return item.AsDto();
        }

        // POST /items
        [HttpPost]
        public ActionResult<ItemDto> CreateItem(CreateItemDto itemDto) 
        {
            Item item = new () 
            {
                Id = Guid.NewGuid(),
                Name = itemDto.Name,
                Price = itemDto.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };

            repository.CreateItemAsync(item);

            return CreatedAtAction(nameof(GetItem), new { id = item.Id }, item.AsDto());
        }

        [HttpPut("{id}")]
        public ActionResult UpdateItem(Guid id, UpdateItemDto itemDto) 
        {
            var existingItem = repository.GetItemAsync(id);

            if(existingItem is null) {
                return NotFound();
            }

            Item updatedItem = existingItem with {
                Name = itemDto.Name,
                Price = itemDto.Price
            };

            repository.UpdateItemAsync(updatedItem);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteItem(Guid id)
        {
            var existingItem = repository.GetItemAsync(id);

            if(existingItem is null) {
                return NotFound();
            }

            repository.DeleteItemAsync(id);

            return NoContent();
        }
    }
}