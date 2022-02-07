using System;

namespace ECommerceBackend.Web.Endpoints.ProductStockUpdateEndpoints
{
    public class ListProductStockUpdateRequest
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
