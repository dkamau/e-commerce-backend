using ECommerceBackend.Core.Entities.ProductEntities;

namespace ECommerceBackend.UnitTests
{
    // Learn more about test builders:
    // https://ardalis.com/improve-tests-with-the-builder-pattern-for-test-data
    public class ProductBuilder
    {
        private Product _product = new Product();

        public ProductBuilder Id(int id)
        {
            _product.Id = id;
            return this;
        }

        public ProductBuilder Title(string name)
        {
            _product.Name = name;
            return this;
        }

        public ProductBuilder Description(string description)
        {
            _product.Description = description;
            return this;
        }

        public ProductBuilder WithDefaultValues()
        {
            _product = new Product() { Id = 1, Name = "Test Item", Description = "Test Description" };

            return this;
        }

        public Product Build() => _product;
    }
}
