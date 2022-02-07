using ECommerceBackend.Core.Entities.OrderEntities;

namespace ECommerceBackend.Core.Specifications.OrderSpecifications
{
    public class OrderFilter : Order
    {
        public string Search { get; set; }
        public int? Page { get; set; }
        public int? RecordsPerPage { get; set; }
    }
}
