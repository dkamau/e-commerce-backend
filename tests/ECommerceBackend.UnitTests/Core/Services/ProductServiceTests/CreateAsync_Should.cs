using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using ECommerceBackend.Core.Entities.ProductEntities;
using ECommerceBackend.Core.Exceptions;
using ECommerceBackend.Core.Interfaces;
using ECommerceBackend.Core.Services;
using ECommerceBackend.Core.Specifications.ProductSpecifications;
using ECommerceBackend.SharedKernel.Interfaces;
using ECommerceBackend.UnitTests.Core.Services.ProductServiceTests.Factory;
using Moq;
using Xunit;

namespace ECommerceBackend.UnitTests.Core.Services.ProductServiceTests
{
    public class CreateAsync_Should
    {
        [Fact]
        public async Task Call_AddAsync_Method_Once()
        {
            // Arrange
            Product product = GenerateNewProduct();

            Mock<IRepository> repository = MockIRepositoryWithAddAsyncReturning(product);

            ProductService productService = ProductServiceFactory.Create(repository);

            // Act
            var result = await productService.CreateAsync(product);

            // Assert
            repository.Verify(n => n.AddAsync(It.IsAny<Product>()), Times.Once, "AddAsync() method to add item to the database was not called.");
        }

        [Fact]
        public async Task Generate_A_Slug()
        {
            // Arrange
            Product product = GenerateNewProduct();

            Mock<IRepository> repository = MockIRepositoryWithAddAsyncReturning(product);

            ProductService productService = ProductServiceFactory.Create(repository);

            // Act
            var result = await productService.CreateAsync(product);

            // Assert
            Assert.False(string.IsNullOrEmpty(result.Slug), "The create method should generate a slug for the product before adding it to the database.");
        }

        [Fact]
        public async Task Generate_A_Valid_Slug()
        {
            // Arrange
            Product product = GenerateNewProduct();

            Mock<IRepository> repository = MockIRepositoryWithAddAsyncReturning(product);

            ProductService productService = ProductServiceFactory.Create(repository);

            // Act
            var result = await productService.CreateAsync(product);

            // Assert
            Assert.Equal($"{product.Name}".ToLower(), result.Slug);
        }

        [Fact]
        public async Task Add_A_Record_To_The_Database_And_Return_It()
        {
            // Arrange
            Product product = GenerateNewProduct();
            Product createdProduct = new Product()
            {
                Id = 1,
            };

            Mock<IRepository> repository = MockIRepositoryWithAddAsyncReturning(createdProduct);

            ProductService productService = ProductServiceFactory.Create(repository);

            // Act
            var result = await productService.CreateAsync(product);

            // Assert
            Assert.IsType<Product>(result);
            Assert.True(result.Id > 0, "Product Id should be greater than 0 after successful addition to the database.");
        }


        [Fact]
        public async Task Throw_CustomException_If_Record_AlreadyExists()
        {
            // Arrange
            Product product = GenerateNewProduct();
            Product existingProduct = new Product()
            {
                Id = 1,
                Name = product.Name
            };

            Mock<IRepository> repository = new Mock<IRepository>();
            repository
               .Setup(n => n.FirstOrDefaultAsync(It.IsAny<GetProducts>()))
               .Returns(Task.FromResult(existingProduct));

            ProductService productService = ProductServiceFactory.Create(repository);

            // Act
            // Assert
            var exception = await Assert.ThrowsAsync<CustomException>(() => productService.CreateAsync(product));
            Assert.Equal(ExceptionCode.ProductAlreadyExists.ToString(), exception.ExceptionCode);
        }

        private Fixture GetFixture()
        {
            Fixture fixture = new Fixture();
            fixture.Customize(new AutoMoqCustomization());
            return fixture;
        }

        private Product GenerateNewProduct()
        {
            Fixture fixture = GetFixture();
            Product product = fixture.Build<Product>()
                .With(n => n.Id, 0)
                .With(n => n.Slug, "").Create();

            return product;
        }

        private Mock<IRepository> MockIRepositoryWithAddAsyncReturning(Product product)
        {
            Mock<IRepository> repository = new Mock<IRepository>();
            repository
                .Setup(n => n.AddAsync(It.IsAny<Product>()))
                .Returns(Task.FromResult(product));

            return repository;
        }
    }
}
