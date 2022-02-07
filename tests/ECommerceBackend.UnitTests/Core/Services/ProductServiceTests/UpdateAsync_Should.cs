using System;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using ECommerceBackend.Core.Entities.ProductEntities;
using ECommerceBackend.Core.Exceptions;
using ECommerceBackend.Core.Services;
using ECommerceBackend.Core.Specifications.ProductSpecifications;
using ECommerceBackend.SharedKernel.Interfaces;
using ECommerceBackend.UnitTests.Core.Services.ProductServiceTests.Factory;
using Moq;
using Xunit;

namespace ECommerceBackend.UnitTests.Core.Services.ProductServiceTests
{
    public class UpdateAsync_Should
    {
        [Fact]
        public async Task Call_UpdateAsync_Method_Once()
        {
            // Arrange
            Product product = GenerateExistingProduct();

            Mock<IRepository> repository = new Mock<IRepository>();

            ProductService productService = ProductServiceFactory.Create(repository);

            // Act
            var result = await productService.UpdateAsync(product);

            // Assert
            repository.Verify(n => n.UpdateAsync(It.IsAny<Product>()), Times.Once, "UpdateAsync() method to update an item in the database was not called.");
        }

        [Fact]
        public async Task Update_Record_And_Return_It()
        {
            // Arrange
            Product product = GenerateExistingProduct();

            Mock<IRepository> repository = new Mock<IRepository>();
            repository
                .Setup(n => n.UpdateAsync(product))
                .Returns(Task.FromResult(product));

            ProductService productService = ProductServiceFactory.Create(repository);

            // Act
            var result = await productService.UpdateAsync(product);

            // Assert
            Assert.IsType<Product>(result);
            Assert.Equal(product.Id, result.Id);
        }

        [Fact]
        public async Task Update_DateUpdated()
        {
            // Arrange
            Product product = GenerateExistingProduct();
            product.DateUpdated = DateTime.UtcNow.AddMinutes(-1);

            DateTime dateBeforeUpdate = product.DateUpdated;

            Mock<IRepository> repository = new Mock<IRepository>();
            repository
                .Setup(n => n.UpdateAsync(product))
                .Returns(Task.FromResult(product));

            ProductService productService = ProductServiceFactory.Create(repository);

            // Act
            var result = await productService.UpdateAsync(product);

            // Assert
            Assert.True(result.DateUpdated > dateBeforeUpdate, "DateUpdated was not updated");
        }

        [Theory()]
        [InlineData(0)]
        [InlineData(-10)]
        public async Task Throw_CustomException_If_Id_Is_InvalidId(int id)
        {
            // Arrange
            Product product = GenerateExistingProduct();
            product.Id = id;

            ProductService productService = ProductServiceFactory.Create(new Mock<IRepository>());

            // Act
            // Assert
            var exception = await Assert.ThrowsAsync<CustomException>(() => productService.UpdateAsync(product));
            Assert.Equal(ExceptionCode.InvalidProductId.ToString(), exception.ExceptionCode);
        }

        [Fact]
        public async Task Throw_CustomException_If_Record_AlreadyExists()
        {
            // Arrange
            Product product = GenerateExistingProduct();
            Product existingProduct = new Product()
            {
                Id = product.Id + 1,
                Name = product.Name,
            };

            Mock<IRepository> repository = new Mock<IRepository>();
            repository
                .Setup(n => n.FirstOrDefaultAsync(It.IsAny<GetProducts>()))
                .Returns(Task.FromResult(existingProduct));

            ProductService productService = ProductServiceFactory.Create(repository);

            // Act
            // Assert
            var exception = await Assert.ThrowsAsync<CustomException>(() => productService.UpdateAsync(product));
            Assert.Equal(ExceptionCode.ProductAlreadyExists.ToString(), exception.ExceptionCode);
        }

        private Product GenerateExistingProduct()
        {
            Fixture fixture = new Fixture();
            fixture.Customize(new AutoMoqCustomization());
            Product product = fixture.Create<Product>();

            return product;
        }
    }
}
