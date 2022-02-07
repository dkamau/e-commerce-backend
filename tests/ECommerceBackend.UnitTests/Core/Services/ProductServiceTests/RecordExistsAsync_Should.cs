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
    public class RecordExistsAsync_Should
    {
        [Theory()]
        [InlineData(0)]
        [InlineData(-10)]
        public async Task Return_False_If_Id_Is_Invalid(int id)
        {
            // Arrange
            ProductService productService = ProductServiceFactory.Create(new Mock<IRepository>());

            // Act
            // Assert
            var result = await productService.RecordExistsAsync(id);
            Assert.False(result);
        }

        [Fact]
        public async Task Return_False_If_Record_DoesNotExist()
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
            var result = await productService.RecordExistsAsync(productId);
            Assert.False(result);
        }

        [Fact]
        public async Task Return_True_If_Record_Exists()
        {
            // Arrange
            int productId = 1;

            Fixture fixture = new Fixture();
            fixture.Customize(new AutoMoqCustomization());
            Product product = fixture.Build<Product>().With(n => n.Id, productId).Create();

            Mock<IRepository> repository = new Mock<IRepository>();
            repository
                .Setup(n => n.FirstOrDefaultAsync(It.IsAny<GetProducts>()))
                .Returns(Task.FromResult(product));

            ProductService productService = ProductServiceFactory.Create(repository);

            // Act
            // Assert
            var result = await productService.RecordExistsAsync(productId);
            Assert.True(result);
        }
    }
}
