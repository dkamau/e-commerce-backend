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
    public class GetByIdAsync_Should
    {
        [Fact]
        public async Task Call_FirstOrDefaultAsync_Method_Once()
        {
            // Arrange
            Product product = GenerateExistingProduct();

            Mock<IRepository> repository = new Mock<IRepository>();
            repository
                .Setup(n => n.FirstOrDefaultAsync(It.IsAny<GetProducts>()))
                .Returns(Task.FromResult(product));

            ProductService productService = ProductServiceFactory.Create(repository);

            // Act
            var result = await productService.GetByIdAsync(product.Id);

            // Assert
            repository.Verify(n => n.FirstOrDefaultAsync(It.IsAny<GetProducts>()), Times.Once);
        }

        [Fact]
        public async Task Return_A_Single_Valid_Record()
        {
            // Arrange
            Product product = GenerateExistingProduct();

            Mock<IRepository> repository = new Mock<IRepository>();
            repository
                .Setup(n => n.FirstOrDefaultAsync(It.IsAny<GetProducts>()))
                .Returns(Task.FromResult(product));

            ProductService productService = ProductServiceFactory.Create(repository);

            // Act
            var result = await productService.GetByIdAsync(product.Id);

            // Assert
            Assert.IsType<Product>(result);
            Assert.Equal(product.Id, result.Id);
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
            var exception = await Assert.ThrowsAsync<CustomException>(() => productService.GetByIdAsync(id));
            Assert.Equal(ExceptionCode.InvalidProductId.ToString(), exception.ExceptionCode);
        }

        [Fact]
        public async Task Throw_CustomException_If_Record_Is_NotFound()
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
            var exception = await Assert.ThrowsAsync<CustomException>(() => productService.GetByIdAsync(productId));
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
