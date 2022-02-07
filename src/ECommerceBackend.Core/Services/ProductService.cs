using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerceBackend.Core.Entities.ProductEntities;
using ECommerceBackend.Core.Exceptions;
using ECommerceBackend.Core.Helpers;
using ECommerceBackend.Core.Interfaces;
using ECommerceBackend.Core.Specifications.ProductSpecifications;
using ECommerceBackend.SharedKernel.Interfaces;

namespace ECommerceBackend.Core.Services
{
    public class ProductService : ICrudService<Product>
    {
        protected readonly IRepository _repository;

        public ProductService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> CountAsync(object filter)
        {
            return await _repository.CountAsync(new GetProducts((ProductFilter)filter));
        }

        public async Task<Product> CreateAsync(Product product)
        {
            if (!await ProductExists(product))
            {
                product.Slug = SlugHelper.Slugify($"{product.Name}");
                return await _repository.AddAsync(product);
            }

            return null;
        }

        public Task DeleteAsync(int accountId)
        {
            throw new NotImplementedException();
        }

        public async Task<Product> GetByIdAsync(int productId)
        {
            return await GetProduct(productId);
        }

        public async Task<Product> GetBySlugAsync(string slug)
        {
            return await GetProduct(slug);
        }

        public async Task<List<Product>> ListAsync(object filter)
        {
            return await _repository.ListAsync(new GetProducts((ProductFilter)filter));
        }

        public async Task<bool> RecordExistsAsync(int productId)
        {
            try
            {
                var product = await GetProduct(productId);
                return product != null;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<Product> SoftDeleteAsync(int productId)
        {
            Product product = await GetByIdAsync(productId);

            product.Deleted = true;
            await _repository.UpdateAsync(product);

            return product;
        }

        public async Task<Product> UpdateAsync(Product product)
        {
            if (product.Id <= 0)
                throw new CustomException(ExceptionCode.InvalidProductId);

            var existingProduct = await _repository.FirstOrDefaultAsync(new GetProducts(new ProductFilter()
            {
                Name = product.Name
            }));

            if (existingProduct != null)
                if (product.Id != existingProduct.Id)
                    throw new CustomException(ExceptionCode.ProductAlreadyExists);

            product.DateUpdated = DateTime.UtcNow;
            await _repository.UpdateAsync(product);

            return product;
        }

        private async Task<Product> GetProduct(int? productId)
        {
            if (productId <= 0)
                throw new CustomException(ExceptionCode.InvalidProductId);

            Product product = await _repository.FirstOrDefaultAsync(new GetProducts(new ProductFilter()
            {
                Id = (int)productId
            }));

            if (product == null)
                throw new CustomException(ExceptionCode.ProductNotFound);

            return product;
        }

        private async Task<Product> GetProduct(string slug)
        {
            Product product = await _repository.FirstOrDefaultAsync(new GetProducts(new ProductFilter()
            {
                Slug = slug
            }));

            if (string.IsNullOrEmpty(slug))
                throw new CustomException(ExceptionCode.NullReference, ExceptionHelper.GetNullExceptionMessage("Slug"));

            if (product == null)
                throw new CustomException(ExceptionCode.ProductNotFound);

            return product;
        }


        private async Task<bool> ProductExists(Product product)
        {
            Product existingProduct = await _repository.FirstOrDefaultAsync(new GetProducts(new ProductFilter()
            {
                Name = product.Name
            }));

            if (existingProduct != null)
                throw new CustomException(ExceptionCode.ProductAlreadyExists);

            return false;
        }
    }
}
