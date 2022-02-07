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
    public class SoftDeleteAsync_Should
    {
        [Fact]
        public async Task Call_UpdateAsync_Method_Once()
        {
            // Arrange
            Product product = GenerateExistingProduct();

            Mock<IRepository> repository = new Mock<IRepository>();
            repository
                .Setup(n => n.FirstOrDefaultAsync(It.IsAny<GetProducts>()))
                .Returns(Task.FromResult(product));

            ProductService productService = ProductServiceFactory.Create(repository);

            // Act
            var result = await productService.SoftDeleteAsync(product.Id);

            // Assert
            repository.Verify(n => n.UpdateAsync(It.IsAny<Product>()), Times.Once, "UpdateAsync() method to update an item in the database was not called.");
        }

        [Fact]
        public async Task Set_Deleted_Property_To_True_And_Return_Record()
        {
            // Arrange
            Product product = GenerateExistingProduct();
            product.Deleted = false;
            Product productAfterDeletion = new Product()
            {
                Id = product.Id,
                Deleted = true
            };

            Mock<IRepository> repository = new Mock<IRepository>();
            repository
               .Setup(n => n.FirstOrDefaultAsync(It.IsAny<GetProducts>()))
               .Returns(Task.FromResult(product));
            repository
                .Setup(n => n.UpdateAsync(product))
                .Returns(Task.FromResult(productAfterDeletion));

            ProductService productService = ProductServiceFactory.Create(repository);

            // Act
            var result = await productService.SoftDeleteAsync(product.Id);

            // Assert
            Assert.IsType<Product>(result);
            Assert.True(result.Deleted);
        }

        [Theory()]
        [InlineData(0)]
        [InlineData(-10)]
        public async Task Throw_CustomException_If_Id_Is_InvalidId(int id)
        {
            // Arrange
            ProductService productService = ProductServiceFactory.Create(new Mock<IRepository>());

            // Act
            // Assert
            var exception = await Assert.ThrowsAsync<CustomException>(() => productService.SoftDeleteAsync(id));
            Assert.Equal(ExceptionCode.InvalidProductId.ToString(), exception.ExceptionCode);
        }

        [Fact]
        public async Task Throw_CustomException_If_Record_NotFound()
        {
            // Arrange
            int productId = 1;

            Mock<IRepository> repository = new Mock<IRepository>();
            repository
                .Setup(n => n.FirstOrDefaultAsync(It.IsAny<GetProducts>()))
                .Returns(Task.FromResult((Product)null));

            ProductService productService = ProductServiceFactory.Create(repository);

            // Act
            // Assert
            var exception = await Assert.ThrowsAsync<CustomException>(() => productService.SoftDeleteAsync(productId));
            Assert.Equal(ExceptionCode.ProductNotFound.ToString(), exception.ExceptionCode);
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
