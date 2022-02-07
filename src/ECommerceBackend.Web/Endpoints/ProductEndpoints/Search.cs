using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using ECommerceBackend.Core.Entities.ProductEntities;
using ECommerceBackend.Core.Exceptions;
using ECommerceBackend.Core.Interfaces;
using ECommerceBackend.Core.Specifications.ProductSpecifications;
using ECommerceBackend.Web.ApiErrors;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ECommerceBackend.Web.Endpoints.ProductEndpoints
{
    [Route("/Products")]
    public class Search : BaseAsyncEndpoint<SearchProductRequest, ProductResponse>
    {
        private readonly ICrudService<Product> _productService;

        public Search(ICrudService<Product> productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Get's a search of Products",
            Description = "Get's a search of Products",
            OperationId = "Product.Search",
            Tags = new[] { "Product Endpoints" })
        ]
        public override async Task<ActionResult<ProductResponse>> HandleAsync([FromQuery] SearchProductRequest request, CancellationToken cancellationToken)
        {
            try
            {
                ProductFilter productFilter = new ProductFilter()
                {
                    Search = request.Search,
                    Name = request.Name,
                };
                int totalRecords = await _productService.CountAsync(productFilter);

                productFilter.Page = request.Page;
                productFilter.RecordsPerPage = request.RecordsPerPage;
                List<Product> products = await _productService.ListAsync(productFilter);


                if (request.Paginate)
                    return Ok(new ListProductResponse(products.OrderBy(n => n.Name).OrderByDescending(n => n.IsActive).ToList(), request.Page, request.RecordsPerPage, totalRecords));
                else
                    return Ok(products.OrderBy(n => n.Name).OrderByDescending(n => n.IsActive).ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestError(ex.Message, ExceptionCode.InternalServerError.ToString()));
            }
        }
    }
}
