using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using ECommerceBackend.Core.Entities.ProductEntities;
using ECommerceBackend.Core.Services;
using ECommerceBackend.Core.Specifications.ProductSpecifications;
using ECommerceBackend.SharedKernel.Interfaces;
using ECommerceBackend.UnitTests.Core.Services.ProductServiceTests.Factory;
using Moq;
using Xunit;

namespace ECommerceBackend.UnitTests.Core.Services.ProductServiceTests
{
    public class ListAsync_Should
    {
        [Fact]
        public async Task Call_ListAsync_Method_Once()
        {
            // Arrange
            // Arrange
            Fixture fixture = new Fixture();
            fixture.Customize(new AutoMoqCustomization());
            List<Product> products = fixture.Create<List<Product>>();

            Mock<IRepository> repository = new Mock<IRepository>();
            repository
                .Setup(n => n.ListAsync(It.IsAny<GetProducts>()))
                .Returns(Task.FromResult(products));

            ProductService productService = ProductServiceFactory.Create(repository);

            // Act
            var result = await productService.ListAsync(fixture.Create<ProductFilter>());

            // Assert
            repository.Verify(n => n.ListAsync(It.IsAny<GetProducts>()), Times.Once, "ListAsync() method to list products was not called.");
        }

        [Fact]
        public async Task Return_A_List_Of_Records()
        {
            // Arrange
            Fixture fixture = new Fixture();
            fixture.Customize(new AutoMoqCustomization());
            List<Product> products = fixture.Create<List<Product>>();

            Mock<IRepository> repository = new Mock<IRepository>();
            repository
                .Setup(n => n.ListAsync(It.IsAny<GetProducts>()))
                .Returns(Task.FromResult(products));

            ProductService productService = ProductServiceFactory.Create(repository);

            // Act
            var result = await productService.ListAsync(fixture.Create<ProductFilter>());

            // Assert
            Assert.IsType<List<Product>>(result);
            Assert.NotNull(result);
        }
    }
}
