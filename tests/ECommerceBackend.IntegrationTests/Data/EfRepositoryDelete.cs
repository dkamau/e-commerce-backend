using System;
using System.Threading.Tasks;
using ECommerceBackend.Core.Entities.ProductEntities;
using ECommerceBackend.UnitTests;
using Xunit;

namespace ECommerceBackend.IntegrationTests.Data
{
    public class EfRepositoryDelete : BaseEfRepoTestFixture
    {
        [Fact]
        public async Task DeletesItemAfterAddingIt()
        {
            // add an item
            var repository = GetRepository();
            var initialTitle = Guid.NewGuid().ToString();
            var item = new ProductBuilder().Title(initialTitle).Build();
            await repository.AddAsync(item);

            // delete the item
            await repository.DeleteAsync(item);

            // verify it's no longer there
            Assert.DoesNotContain(await repository.ListAsync<Product>(),
                i => i.Name == initialTitle);
        }
    }
}
