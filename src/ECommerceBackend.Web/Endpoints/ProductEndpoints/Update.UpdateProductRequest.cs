using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace ECommerceBackend.Web.Endpoints.ProductEndpoints
{
    public class UpdateProductRequest
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string PhotoUrl { get; set; }
        public string Description { get; set; }
        public decimal? BuyingPrice { get; set; }
        public decimal? SellingPrice { get; set; }
        public bool IsActive { get; set; }
    }
}
