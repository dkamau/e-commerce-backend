using System.ComponentModel.DataAnnotations;

namespace ECommerceBackend.Web.Endpoints.AuthenticationEndpoints
{
    public class ResetPasswordRequest
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
