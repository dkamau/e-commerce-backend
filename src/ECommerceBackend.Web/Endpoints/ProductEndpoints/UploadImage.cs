using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using ECommerceBackend.Core.Entities.ProductEntities;
using ECommerceBackend.Core.Exceptions;
using ECommerceBackend.Core.Interfaces;
using ECommerceBackend.SharedKernel.Interfaces;
using ECommerceBackend.Web.ApiErrors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ECommerceBackend.Web.Endpoints.ProductEndpoints
{
    [Route("/Products")]
    public class UploadImage : BaseAsyncEndpoint<IFormFile, ProductResponse>
    {
        protected readonly ICrudService<Product> _productService;
        protected readonly IImageService _imageService;
        protected readonly IRepository _repository;

        public UploadImage(ICrudService<Product> productService, IImageService imageService, IRepository repository)
        {
            _productService = productService;
            _imageService = imageService;
            _repository = repository;
        }

        [FromRoute]
        public int productId { get; set; }

        [HttpPost("{productId:int}/UploadImage")]
        [SwaggerOperation(
            Summary = "Uploads a Product's image",
            Description = "Uploads a Product's image",
            OperationId = "Products.UploadImage",
            Tags = new[] { "Product Endpoints" })
        ]
        public override async Task<ActionResult<ProductResponse>> HandleAsync(IFormFile file, CancellationToken cancellationToken)
        {
            try
            {
                Product product = await _productService.GetByIdAsync(productId);

                //IFormFile file = Request.Form.Files[0];

                if (file == null)
                    return BadRequest("Missing file");

                string fileExtension = Path.GetExtension(file.FileName).ToLower();

                if (!fileExtension.Equals(".png") &&
                    !fileExtension.Equals(".gif") &&
                    !fileExtension.Equals(".jpg") &&
                    !fileExtension.Equals(".jpeg"))
                {
                    throw new CustomException(ExceptionCode.InvalidImageFile, "Please provide a valid image file.");
                }

                if(file.Length > 1000000)
                    throw new CustomException(ExceptionCode.FileTooLarge, "Please upload a file that is less than 1Mb in size.");

                string fileName = $"product_image_{productId}{fileExtension}";

                var mediaFileDeleted = await _imageService.DeleteFileAsync(product.ImageFileId);
                var resp = await _imageService.UploadFileAsync(file.OpenReadStream(), fileName);

                product.PhotoUrl = resp.URL;
                product.ImageFileId = resp.UploadedFileId;

                await _repository.UpdateAsync(product);
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

    public class FileUploadResponse
    {
        public string FileUrl { get; set; }
    }
}
