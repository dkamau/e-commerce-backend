using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerceBackend.Core.Entities.ProductEntities;
using ECommerceBackend.Core.Interfaces;
using ECommerceBackend.Core.Specifications.ProductStockUpdateSpecifications;
using ECommerceBackend.SharedKernel.Interfaces;

namespace ECommerceBackend.Core.Services
{
    public class ProductStockUpdateService : IProductStockUpdateService
    {
        protected readonly IRepository _repository;
        public ProductStockUpdateService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<ProductStockUpdate> CreateAsync(ProductStockUpdate productStockUpdate)
        {
            return await _repository.AddAsync(productStockUpdate);
        }

        public async Task<List<ProductStockUpdate>> CreateAsync(List<ProductStockUpdate> productStockUpdates)
        {
            return await _repository.AddRangeAsync(productStockUpdates);
        }

        public async Task<List<ProductStockUpdate>> ListAsync(int productId, DateTime? startDate, DateTime? endDate, int? numberOfRecords = null)
        {
            List<ProductStockUpdate> productStockUpdates = await _repository.ListAsync(new GetProductStockUpdates(productId, startDate, endDate, numberOfRecords));
            return productStockUpdates;
        }
    }
}
