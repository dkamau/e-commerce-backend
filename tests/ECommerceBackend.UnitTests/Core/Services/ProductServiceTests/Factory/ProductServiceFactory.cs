using ECommerceBackend.Core.Interfaces;
using ECommerceBackend.Core.Services;
using ECommerceBackend.SharedKernel.Interfaces;
using Moq;

namespace ECommerceBackend.UnitTests.Core.Services.ProductServiceTests.Factory
{
    internal class ProductServiceFactory
    {
        internal static ProductService Create(
           Mock<IRepository> repository)
        {
            return new ProductService(repository.Object);
        }
    }
}
