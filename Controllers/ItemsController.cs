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
            return repository.GetItems().Select(item => item.AsDto());
        }

        [HttpGet("{id}")]
        public ActionResult<ItemDto> GetItem(Guid id) 
        {
            var item = repository.GetItem(id);
            
            if(item is null) {
                return NotFound();
            }

            return item.AsDto();
        }
    }
}