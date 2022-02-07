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
    public class Create : BaseAsyncEndpoint<ProductRequest, ProductResponse>
    {
        private readonly ICrudService<Product> _productService;

        public Create(ICrudService<Product> productService)
        {
            _productService = productService;
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Creates a new Product",
            Description = "Creates a new Product",
            OperationId = "Product.Create",
            Tags = new[] { "Product Endpoints" })
        ]
        public override async Task<ActionResult<ProductResponse>> HandleAsync(ProductRequest request, CancellationToken cancellationToken)
        {
            try
            {
                Product product = await _productService.CreateAsync(new Product()
                {
                    Name = request.Name,
                    Description = request.Description,
                    BuyingPrice = request.BuyingPrice,
                    SellingPrice = request.SellingPrice,
                });

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
