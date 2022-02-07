using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerceBackend.Core.Entities.ProductEntities;

namespace ECommerceBackend.Core.Interfaces
{
    public interface IProductStockUpdateService
    {
        Task<List<ProductStockUpdate>> ListAsync(int productId, DateTime? startDate, DateTime? endDate, int? numberOfRecords = null);
        Task<ProductStockUpdate> CreateAsync(ProductStockUpdate productStockUpdate);
        Task<List<ProductStockUpdate>> CreateAsync(List<ProductStockUpdate> productStockUpdates);
    }
}
