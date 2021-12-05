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
            result.Value.Should().BeEquivalentTo(
                expectedItem,
                options => options.ComparingByMembers<Item>());
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
            actualItems.Should().BeEquivalentTo(
                expectedItems,
                options => options.ComparingByMembers<Item>());
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
