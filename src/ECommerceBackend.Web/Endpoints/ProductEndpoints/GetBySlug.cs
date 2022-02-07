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
    public class GetBySlug : BaseAsyncEndpoint<string, ProductResponse>
    {
        private readonly ICrudService<Product> _productService;

        public GetBySlug(ICrudService<Product> productService)
        {
            _productService = productService;
        }

        [HttpGet("{slug}")]
        [SwaggerOperation(
            Summary = "Get's a single Product by slug",
            Description = "Get's a single Product by slug",
            OperationId = "Product.GetBySlug",
            Tags = new[] { "Product Endpoints" })
        ]
        public override async Task<ActionResult<ProductResponse>> HandleAsync(string slug, CancellationToken cancellationToken)
        {
            try
            {
                Product product = await _productService.GetBySlugAsync(slug);
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
