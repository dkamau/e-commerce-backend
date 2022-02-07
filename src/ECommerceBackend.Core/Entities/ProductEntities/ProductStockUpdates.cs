using ECommerceBackend.SharedKernel;

namespace ECommerceBackend.Core.Entities.ProductEntities
{
    public class ProductStockUpdate : BaseEntity
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public ProductStockUpdateType ProductStockUpdateType { get; set; }
    }

    public enum ProductStockUpdateType
    {
        Addition,
        Deduction
    }
}
