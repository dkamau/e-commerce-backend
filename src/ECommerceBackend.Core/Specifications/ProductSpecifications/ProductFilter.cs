using ECommerceBackend.Core.Entities.ProductEntities;

namespace ECommerceBackend.Core.Specifications.ProductSpecifications
{
    public class ProductFilter : Product
    {
        public string Search { get; set; }
        public int? Page { get; set; }
        public int? RecordsPerPage { get; set; }
    }
}
