using Ardalis.Specification;
using ECommerceBackend.Core.Entities.ProductEntities;
using Microsoft.EntityFrameworkCore;

namespace ECommerceBackend.Core.Specifications.ProductSpecifications
{
    public class GetProducts : Specification<Product>
    {
        public GetProducts(ProductFilter filter)
        {
            if (filter.Id > 0)
                Query.Where(n => n.Id == filter.Id);

            if (!string.IsNullOrEmpty(filter.Slug))
                Query.Where(n => n.Slug == filter.Slug.ToLower());

            if (!string.IsNullOrEmpty(filter.Name))
                Query.Where(n => n.Name.ToLower() == filter.Name.ToLower());

            if (!string.IsNullOrEmpty(filter.Search))
                Query.Where(n =>
                EF.Functions.Like(n.Name.ToLower(), $"%{filter.Search.ToLower()}%") ||
                EF.Functions.Like(n.Description.ToLower(), $"%{filter.Search.ToLower()}%"));

            Query.Include(n => n.ProductStockUpdates);
        }
    }
}
