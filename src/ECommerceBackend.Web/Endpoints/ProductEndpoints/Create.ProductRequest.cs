using System.ComponentModel.DataAnnotations;

namespace ECommerceBackend.Web.Endpoints.ProductEndpoints
{
    public class ProductRequest
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
        public decimal? BuyingPrice { get; set; }
        public decimal? SellingPrice { get; set; }
    }
}
