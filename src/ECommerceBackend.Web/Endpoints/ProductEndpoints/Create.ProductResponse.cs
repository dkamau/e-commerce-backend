using System;
using System.Linq;
using ECommerceBackend.Core.Entities.ProductEntities;

namespace ECommerceBackend.Web.Endpoints.ProductEndpoints
{
    public class ProductResponse
    {
        public int Id { get; set; }
        public string PhotoUrl { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal? BuyingPrice { get; set; }
        public decimal? SellingPrice { get; set; }
        public string Slug { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }

        public static ProductResponse Create(Product product)
        {
            ProductResponse productResponse = new ProductResponse()
            {
                Id = product.Id,
                PhotoUrl = product.PhotoUrl,
                Name = product.Name,
                Description = product.Description,
                BuyingPrice = product.BuyingPrice,
                SellingPrice = product.SellingPrice,
                Slug = product.Slug,
                IsActive = product.IsActive,
                DateCreated = product.DateCreated,
                DateUpdated = product.DateUpdated
            };

            if (product.ProductStockUpdates != null)
            {
                int totalAdditions = product.ProductStockUpdates.Where(n => n.ProductStockUpdateType == ProductStockUpdateType.Addition).Sum(n => n.Quantity);
                int totalDeductions = product.ProductStockUpdates.Where(n => n.ProductStockUpdateType == ProductStockUpdateType.Deduction).Sum(n => n.Quantity);
                int quantity = totalAdditions - totalDeductions;
                productResponse.Quantity = quantity;
            }

            return productResponse;
        }
    }
}
