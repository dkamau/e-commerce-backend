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
    public class Update : BaseAsyncEndpoint<UpdateProductRequest, ProductResponse>
    {
        private readonly ICrudService<Product> _productService;

        public Update(ICrudService<Product> productService)
        {
            _productService = productService;
        }

        [HttpPatch]
        [SwaggerOperation(
            Summary = "Updates an Product",
            Description = "Updates an Product",
            OperationId = "Product.Update",
            Tags = new[] { "Product Endpoints" })
        ]
        public override async Task<ActionResult<ProductResponse>> HandleAsync(UpdateProductRequest request, CancellationToken cancellationToken)
        {
            try
            {
                Product product = await _productService.GetByIdAsync(request.Id);
                product.Name = request.Name;
                product.PhotoUrl = request.PhotoUrl;
                product.Description = request.Description;
                product.BuyingPrice = request.BuyingPrice;
                product.SellingPrice = request.SellingPrice;
                product.IsActive = request.IsActive;

                await _productService.UpdateAsync(product);

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
