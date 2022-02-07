using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerceBackend.Core.Entities.OrderEntities;
using ECommerceBackend.Core.Entities.ProductEntities;

namespace ECommerceBackend.Core.Interfaces
{
    public interface IOrderService
    {
        Task<string> GenerateOrderNumber();
        Task<Order> CreateAsync(Order order, List<Product> products);
    }
}
