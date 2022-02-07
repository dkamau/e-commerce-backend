﻿using System;
using System.Linq;
using System.Threading.Tasks;
using ECommerceBackend.Core.Entities.ProductEntities;
using ECommerceBackend.UnitTests;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ECommerceBackend.IntegrationTests.Data
{
    public class EfRepositoryUpdate : BaseEfRepoTestFixture
    {
        [Fact]
        public async Task UpdatesItemAfterAddingIt()
        {
            // add an item
            var repository = GetRepository();
            var initialTitle = Guid.NewGuid().ToString();
            var item = new ProductBuilder().Title(initialTitle).Build();

            await repository.AddAsync(item);

            // detach the item so we get a different instance
            _dbContext.Entry(item).State = EntityState.Detached;

            // fetch the item and update its title
            var newItem = (await repository.ListAsync<Product>())
                .FirstOrDefault(i => i.Name == initialTitle);
            Assert.NotNull(newItem);
            Assert.NotSame(item, newItem);
            var newTitle = Guid.NewGuid().ToString();
            newItem.Name = newTitle;

            // Update the item
            await repository.UpdateAsync(newItem);
            var updatedItem = (await repository.ListAsync<Product>())
                .FirstOrDefault(i => i.Name == newTitle);

            Assert.NotNull(updatedItem);
            Assert.NotEqual(item.Name, updatedItem.Name);
            Assert.Equal(newItem.Id, updatedItem.Id);
        }
    }
}
