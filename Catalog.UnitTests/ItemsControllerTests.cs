using System;
using Xunit;
using Moq;
using Catalog.Api.Repositories;
using Catalog.Api.Models;
using Catalog.Api.Controllers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Catalog.Api.Dtos;
using FluentAssertions;
using System.Collections;
using System.Collections.Generic;

namespace Catalog.UnitTests
{
    public class ItemsControllerTests
    {
        // Fake the repository dependency using the interface 
        private readonly Mock<IItemsRepository> repositoryStub = new();

        private readonly Random rand = new();

        // public void UnitOfWork_StateUnderTest_ExpectedBehaviour
        // stubs are fake versions of dependencies needed for tests 
        [Fact]
        public async Task GetItemAsync_WithUnexistingItem_ReturnsNotFound()
        {
            // Arrange
            repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Item)null);

            var controller = new ItemsController(repositoryStub.Object);

            // Act 
            var result = await controller.GetItemAsync(Guid.NewGuid());

            // Assert 
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task get_item_async_with_existing_item_returns_expected_item()
        {
            // Arrange 
            var expectedItem = CreateRandomItem();

            repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
                .ReturnsAsync(expectedItem);
            
            var controller = new ItemsController(repositoryStub.Object);

            // Act 
            var result = await controller.GetItemAsync(Guid.NewGuid());

            // Assert 
            result.Value.Should().BeEquivalentTo(expectedItem);
        }

        [Fact]
        public async Task GetItemsAsync_SearchByName_ReturnsItemsWithName()
        {
            var allItems = new []
            {
                new Item() { Name = "Potion" },
                new Item() { Name = "Antidote" },
                new Item() { Name = "Hi-Potion" },
            };

            var nameToMatch = "Potion";

            repositoryStub.Setup(repo => repo.GetItemsAsync())
                .ReturnsAsync(allItems);

            var controller = new ItemsController(repositoryStub.Object);

            // Act 
            IEnumerable<ItemDto> foundItems = await controller.GetItemsAsync(nameToMatch);

            foundItems.Should().OnlyContain(
                item => item.Name == allItems[0].Name || item.Name == allItems[2].Name
            );
        }

        [Fact]
        public async Task GetItemsAsync_WithExistingItems_ReturnsAllItems()
        {
            // Arrange 
            var expectedItems = new[] { CreateRandomItem(), CreateRandomItem(), CreateRandomItem() };

            repositoryStub.Setup(repo => repo.GetItemsAsync())
                .ReturnsAsync(expectedItems);
            
            var controller = new ItemsController(repositoryStub.Object);

            // Act 
            var actualItems = await controller.GetItemsAsync();

            // Assert 
            actualItems.Should().BeEquivalentTo(expectedItems);
        }

        [Fact]
        public async Task CreateItemAsync_WithItemToCreate_ReturnsCreatedItem()
        {
            // Arrange
            var itemToCreate = new CreateItemDto(
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString(), 
                rand.Next(1000));
            
            var controller = new ItemsController(repositoryStub.Object);

            // Act
            var result = await controller.CreateItemAsync(itemToCreate);

            // Assert 
            var createdItem = (result.Result as CreatedAtActionResult).Value as ItemDto;

            itemToCreate.Should().BeEquivalentTo(
                createdItem,
                options => options.ComparingByMembers<ItemDto>().ExcludingMissingMembers()
            );
            createdItem.Id.Should().NotBeEmpty();
            createdItem.CreatedDate.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromMilliseconds(1000));
        }

        [Fact]
        public async Task UpdateItemAsync_WithExistingItem_ReturnsNoContent()
        {
            // Arrange
            Item existingItem = CreateRandomItem();

            repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
                .ReturnsAsync(existingItem);

            var itemId = existingItem.Id;
            var itemToUpdate = new UpdateItemDto(
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString(), 
                existingItem.Price + 3
            );
            
            var controller = new ItemsController(repositoryStub.Object);

            var result = await controller.UpdateItemAsync(itemId, itemToUpdate);

            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task DeleteItemAsync_WithExistingItem_ReturnsNoContent()
        {
            // Arrange
            Item existingItem = CreateRandomItem();

            repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
                .ReturnsAsync(existingItem);
            
            // Act 
            var controller = new ItemsController(repositoryStub.Object);

            var result = await controller.DeleteItemAsync(existingItem.Id);

            // Assert 
            result.Should().BeOfType<NoContentResult>();
        }

        private Item CreateRandomItem() 
        {
            return new() 
            {
                Id = Guid.NewGuid(),
                Name = Guid.NewGuid().ToString(),
                Price = rand.Next(1000),
                CreatedDate = DateTimeOffset.UtcNow
            };
        }
    }
}
