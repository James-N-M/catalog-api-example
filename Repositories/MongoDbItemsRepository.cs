using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CatalogApi.Models;
using MongoDB.Driver;

namespace CatalogApi.Repositories
{
    public class MongoDbItemsRepository : IItemsRepository
    {
        private const string databaseName = "catalog";
        private const string collectionName = "items";
        private readonly IMongoCollection<Item> itemsCollection;
        
        public MongoDbItemsRepository(IMongoClient mongoClient)
        {
            IMongoDatabase database = mongoClient.GetDatabase(databaseName);

            itemsCollection = database.GetCollection<Item>(collectionName);
        }
        public void CreateItem(Item item) 
        {
            itemsCollection.InsertOne(item);
        }

        public void UpdateItem(Item item)
        {
            
        }
        public void DeleteItem(Guid id)
        {
            
        }

        public Item GetItem(Guid id) 
        {
            return new Item();
        }

        public IEnumerable<Item> GetItems() 
        {
            return new List<Item>();
        }
    }
}