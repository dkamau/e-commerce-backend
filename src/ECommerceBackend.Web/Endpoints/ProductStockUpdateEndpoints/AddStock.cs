using System;
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
    [Route("/ProductStockUpdates")]
    public class AddStock : BaseAsyncEndpoint<ProductStockUpdateRequest, ProductStockUpdateResponse>
    {
        private readonly IProductStockUpdateService _productStockUpdateService;

        public AddStock(
            IProductStockUpdateService productStockUpdateService)
        {
            _productStockUpdateService = productStockUpdateService;
        }

        [HttpPost("AddStock")]
        [SwaggerOperation(
            Summary = "Adds product stock",
            Description = "Adds product stock",
            OperationId = "ProductStockUpdate.AddStock",
            Tags = new[] { "Product Stock Update Endpoints" })
        ]
        public override async Task<ActionResult<ProductStockUpdateResponse>> HandleAsync(ProductStockUpdateRequest request, CancellationToken cancellationToken)
        {
            try
            {
                ProductStockUpdate productStockUpdate = await _productStockUpdateService.CreateAsync(new ProductStockUpdate()
                {
                    ProductId = request.ProductId,
                    ProductStockUpdateType = ProductStockUpdateType.Addition,
                    Price = request.Price,
                    Quantity = request.Quantity,
                });

                return Ok(ProductStockUpdateResponse.Create(productStockUpdate));
            }
            catch (CustomException ex)
            {
                return new CustomErrorResuslt().Error(ex);
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestError(ex.Message, ExceptionCode.InternalServerError.ToString()));
            }
        }
    }
}
