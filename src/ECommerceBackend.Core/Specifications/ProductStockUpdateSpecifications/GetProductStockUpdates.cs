using System;
using Ardalis.Specification;
using ECommerceBackend.Core.Entities.ProductEntities;

namespace ECommerceBackend.Core.Specifications.ProductStockUpdateSpecifications
{
    public class GetProductStockUpdates : Specification<ProductStockUpdate>
    {
        public GetProductStockUpdates(int productId, DateTime? startDate = null, DateTime? endDate = null, int? numberOfRecords = null)
        {
            if (productId > 0)
                Query.Where(n => n.ProductId == productId);

            if (startDate != null)
                Query.Where(n => n.DateCreated >= startDate);

            if (endDate != null)
                Query.Where(n => n.DateCreated <= endDate);

            if (numberOfRecords != null)
                Query.Take((int)numberOfRecords);
        }
    }
}
