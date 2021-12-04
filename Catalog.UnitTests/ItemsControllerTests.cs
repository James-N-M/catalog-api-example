using System;
using Xunit;
using Moq;
using Catalog.Api.Repositories;
using Catalog.Api.Models;
using Catalog.Api.Controllers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.UnitTests
{
    public class ItemsControllerTests
    {
        // public void UnitOfWork_StateUnderTest_ExpectedBehaviour
        // stubs are fake versions of dependencies needed for tests 
        [Fact]
        public async Task GetItemAsync_WithUnexistingItem_ReturnsNotFound()
        {
            // Arrange
            // Fake the repository dependency using the interface 
            var repositoryStub = new Mock<IItemsRepository>();

            repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Item)null);

            var controller = new ItemsController(repositoryStub.Object);

            // Act 
            var result = await controller.GetItemAsync(Guid.NewGuid());

            // Assert 
            Assert.IsType<NotFoundResult>(result.Result);
        }
    }
}
