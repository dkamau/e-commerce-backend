using ECommerceBackend.SharedKernel;

namespace ECommerceBackend.Core.Entities.OrderEntities
{
    public class OrderedProduct : BaseEntity
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
