using Ardalis.Specification;
using ECommerceBackend.Core.Entities.OrderEntities;
using Microsoft.EntityFrameworkCore;

namespace ECommerceBackend.Core.Specifications.OrderSpecifications
{
    public class GetOrders : Specification<Order>
    {
        public GetOrders(OrderFilter filter)
        {
            if (filter.UserId > 0)
                Query.Where(n => n.UserId == filter.UserId);

            if (!string.IsNullOrEmpty(filter.Search))
                Query.Where(n =>
                EF.Functions.Like(n.OrderNumber.ToLower(), $"%{filter.OrderNumber.ToLower()}%"));
        }
    }
}
