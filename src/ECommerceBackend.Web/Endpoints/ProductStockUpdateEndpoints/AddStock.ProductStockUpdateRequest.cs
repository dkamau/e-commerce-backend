using System.ComponentModel.DataAnnotations;
using ECommerceBackend.Core.Entities.ProductEntities;

namespace ECommerceBackend.Web.Endpoints.ProductStockUpdateEndpoints
{
    public class ProductStockUpdateRequest
    {
        [Required]
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public ProductStockUpdateType ProductStockUpdateType { get; set; }
    }
}
