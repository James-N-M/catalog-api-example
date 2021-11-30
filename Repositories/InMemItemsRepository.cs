using System;
using System.Collections.Generic;
using CatalogApi.Models;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogApi.Repositories
{
    // I commented out this class because it no longer implements the interface
    // but its a good reference for how to mock out a repo and a database when
    // building a applicaiton.
    // to implement the interface and make it async
    // GetItemsAsync()
    // use await Task.FromResult(items); 

    // public class InMemItemsRepository : IItemsRepository
    // {
    //     private readonly List<Item> items = new()
    //     {
    //         new Item { Id = Guid.NewGuid(), Name = "Potion", Price = 0, CreatedDate = DateTimeOffset.UtcNow },
    //         new Item { Id = Guid.NewGuid(), Name = "Iron Sword", Price = 20, CreatedDate = DateTimeOffset.UtcNow },
    //         new Item { Id = Guid.NewGuid(), Name = "Bronze Shield", Price = 18, CreatedDate = DateTimeOffset.UtcNow },
    //     };

    //     public IEnumerable<Item> GetItemsAsync()
    //     {
    //         return items;
    //     }

    //     public Item GetItemAsync(Guid id)
    //     {
    //         return items
    //             .Where(item => item.Id == id)
    //             .SingleOrDefault();
    //     }

    //     public void CreateItemAsync(Item item) 
    //     {
    //         items.Add(item);
    //     }

    //     public void UpdateItemAsync(Item item) 
    //     {
    //         var index = items.FindIndex(existingItem => existingItem.Id == item.Id);

    //         items[index] = item; 
    //     }

    //     public void DeleteItemAsync(Guid id) 
    //     {
    //         var index = items.FindIndex(existingItem => existingItem.Id == id);

    //         items.RemoveAt(index);
    //     }
    // }
}