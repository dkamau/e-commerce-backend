using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using ECommerceBackend.Core.Entities.ProductEntities;
using ECommerceBackend.Core.Exceptions;
using ECommerceBackend.Core.Interfaces;
using ECommerceBackend.Web.ApiErrors;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ECommerceBackend.Web.Endpoints.ProductStockUpdateEndpoints
{
    [Route("/Products")]
    public class List : BaseAsyncEndpoint<ListProductStockUpdateRequest, ProductStockUpdateResponse>
    {
        private readonly IProductStockUpdateService _productStockUpdateService;

        public List(IProductStockUpdateService productStockUpdateService)
        {
            _productStockUpdateService = productStockUpdateService;
        }

        [FromRoute]
        public int productId { get; set; }

        [HttpGet("{productId:int}/ProductStockUpdates")]
        [SwaggerOperation(
            Summary = "Get's a list of product stock updates",
            Description = "Get's a list of product stock updates",
            OperationId = "ProductStockUpdate.List",
            Tags = new[] { "Product Stock Update Endpoints" })
        ]
        public override async Task<ActionResult<ProductStockUpdateResponse>> HandleAsync([FromQuery] ListProductStockUpdateRequest request, CancellationToken cancellationToken)
        {
            try
            {
                List<ProductStockUpdate> productStockUpdates = await _productStockUpdateService.ListAsync(productId, request.StartDate, request.EndDate, 100);

                return Ok(productStockUpdates.Select(n => ProductStockUpdateResponse.Create(n)).OrderByDescending(n => n.DateCreated).ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestError(ex.Message, ExceptionCode.InternalServerError.ToString()));
            }
        }
    }
}
