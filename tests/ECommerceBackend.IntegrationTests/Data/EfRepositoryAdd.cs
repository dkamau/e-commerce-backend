using System.Linq;
using System.Threading.Tasks;
using ECommerceBackend.Core.Entities.ProductEntities;
using ECommerceBackend.UnitTests;
using Xunit;

namespace ECommerceBackend.IntegrationTests.Data
{
    public class EfRepositoryAdd : BaseEfRepoTestFixture
    {
        [Fact]
        public async Task AddsItemAndSetsId()
        {
            var repository = GetRepository();
            var item = new ProductBuilder().Build();

            await repository.AddAsync(item);

            var newItem = (await repository.ListAsync<Product>())
                            .FirstOrDefault();

            Assert.Equal(item, newItem);
            Assert.True(newItem?.Id > 0);
        }
    }
}
