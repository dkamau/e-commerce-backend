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
    public class GetBySlugAsync_Should
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
            var result = await productService.GetBySlugAsync(product.Slug);

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
            var result = await productService.GetBySlugAsync(product.Slug);

            // Assert
            Assert.IsType<Product>(result);
            Assert.Equal(product.Id, result.Id);
        }

        [Theory()]
        [InlineData("")]
        [InlineData(null)]
        public async Task Throw_CustomException_If_Slug_Is_NullOrEmpty(string slug)
        {
            // Arrange
            ProductService productService = ProductServiceFactory.Create(new Mock<IRepository>());

            // Act
            // Assert
            var exception = await Assert.ThrowsAsync<CustomException>(() => productService.GetBySlugAsync(slug));
            Assert.Equal(ExceptionCode.NullReference.ToString(), exception.ExceptionCode);
        }

        [Fact]
        public async Task Throw_CustomException_If_Record_Is_NotFound()
        {
            // Arrange
            Mock<IRepository> repository = new Mock<IRepository>();
            repository
                .Setup(n => n.FirstOrDefaultAsync(It.IsAny<GetProducts>()))
                .Returns(Task.FromResult((Product)null));

            ProductService productService = ProductServiceFactory.Create(repository);

            // Act
            // Assert
            var exception = await Assert.ThrowsAsync<CustomException>(() => productService.GetBySlugAsync("anything"));
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
