using System;
using System.Collections.Generic;
using System.Linq;
using ECommerceBackend.Core.Entities.ProductEntities;
using ECommerceBackend.Web.AbstractClasses;

namespace ECommerceBackend.Web.Endpoints.ProductEndpoints
{
    public class ListProductResponse : Pagination
    {
        public ListProductResponse(List<Product> products, int? page, int? recordsPerPage, int totalRecords)
        {
            Page = page == null ? 1 : (int)page;
            RecordsPerPage = recordsPerPage == null ? products.Count : (int)recordsPerPage;
            TotalRecords = totalRecords;
            TotalPages = TotalRecords > 0 ? (int)Math.Ceiling(TotalRecords / (double)RecordsPerPage) : 0;

            Products = products.Select(n => ProductResponse.Create(n)).ToList();

            RecordsInPage = Products.Count;

            if (Page > 1)
                PreviousPage = Page - 1;
            if (Page > TotalPages)
                PreviousPage = TotalPages;
            if (PreviousPage == 0)
                PreviousPage = null;
            if (Page < TotalPages)
                NextPage = Page + 1;
        }

        public List<ProductResponse> Products { get; set; }
    }
}
