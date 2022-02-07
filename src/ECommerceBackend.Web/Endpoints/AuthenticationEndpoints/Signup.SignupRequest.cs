using System.ComponentModel.DataAnnotations;

namespace ECommerceBackend.Web.Endpoints.AuthenticationEndpoints
{
    public class SignupRequest
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }


        [Required]
        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
