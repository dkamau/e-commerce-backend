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

namespace ECommerceBackend.Web.Endpoints.ProductEndpoints
{
    [Route("/Products")]
    public class GetById : BaseAsyncEndpoint<int, ProductResponse>
    {
        private readonly ICrudService<Product> _productService;

        public GetById(ICrudService<Product> productService)
        {
            _productService = productService;
        }

        [HttpGet("{productId:int}")]
        [SwaggerOperation(
            Summary = "Get's a single Product by Id",
            Description = "Get's a single Product by Id",
            OperationId = "Product.GetById",
            Tags = new[] { "Product Endpoints" })
        ]
        public override async Task<ActionResult<ProductResponse>> HandleAsync(int productId, CancellationToken cancellationToken)
        {
            try
            {
                Product product = await _productService.GetByIdAsync(productId);
                return Ok(ProductResponse.Create(product));
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
