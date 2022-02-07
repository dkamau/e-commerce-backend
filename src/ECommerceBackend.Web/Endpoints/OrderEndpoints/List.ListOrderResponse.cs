using System;
using System.Collections.Generic;
using System.Linq;
using ECommerceBackend.Core.Entities.OrderEntities;
using ECommerceBackend.Web.AbstractClasses;

namespace ECommerceBackend.Web.Endpoints.OrderEndpoints
{
    public class ListOrderResponse : Pagination
    {
        public ListOrderResponse(List<Order> orders, int? page, int? recordsPerPage, int totalRecords)
        {
            Page = page == null ? 1 : (int)page;
            RecordsPerPage = recordsPerPage == null ? orders.Count : (int)recordsPerPage;
            TotalRecords = totalRecords;
            TotalPages = TotalRecords > 0 ? (int)Math.Ceiling(TotalRecords / (double)RecordsPerPage) : 0;

            Orders = orders.Select(n => OrderResponse.Create(n)).ToList();

            RecordsInPage = Orders.Count;

            if (Page > 1)
                PreviousPage = Page - 1;
            if (Page > TotalPages)
                PreviousPage = TotalPages;
            if (PreviousPage == 0)
                PreviousPage = null;
            if (Page < TotalPages)
                NextPage = Page + 1;
        }

        public List<OrderResponse> Orders { get; set; }
    }
}
