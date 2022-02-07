using System.Collections.Generic;
using ECommerceBackend.SharedKernel;

namespace ECommerceBackend.Core.Entities.ProductEntities
{
    public class Product : BaseEntity
    {
        public string PhotoUrl { get; set; }
        public string ImageFileId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal? BuyingPrice { get; set; }
        public decimal? SellingPrice { get; set; }
        public string Slug { get; set; }
        public virtual ICollection<ProductStockUpdate> ProductStockUpdates { get; set; }
    }
}
