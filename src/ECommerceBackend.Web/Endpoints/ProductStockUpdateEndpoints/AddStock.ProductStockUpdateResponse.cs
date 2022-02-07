using System;
using ECommerceBackend.Core.Entities.ProductEntities;

namespace ECommerceBackend.Web.Endpoints.ProductStockUpdateEndpoints
{
    public class ProductStockUpdateResponse
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Total
        {
            get
            {
                return Quantity * Price;
            }
        }
        public ProductStockUpdateType ProductStockUpdateType { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }

        public static ProductStockUpdateResponse Create(ProductStockUpdate productStockUpdate)
        {
            ProductStockUpdateResponse productStockUpdateResponse = new ProductStockUpdateResponse()
            {
                Id = productStockUpdate.Id,
                ProductId = productStockUpdate.ProductId,
                ProductName = "",
                ProductStockUpdateType = productStockUpdate.ProductStockUpdateType,
                Quantity = productStockUpdate.Quantity,
                Price = productStockUpdate.Price,
                DateCreated = productStockUpdate.DateCreated,
                DateUpdated = productStockUpdate.DateUpdated
            };

            return productStockUpdateResponse;
        }
    }
}
