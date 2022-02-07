using ECommerceBackend.Core.Entities.AuthenticationEntities;

namespace ECommerceBackend.Web.Endpoints.AuthenticationEndpoints
{
    public class SignupResponse
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string PhotoUrl { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Token { get; set; }
        public bool EmailIsConfirmed { get; set; } = false;

        public static SignupResponse Create(User user)
        {
            return new SignupResponse()
            {
                UserId = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                EmailIsConfirmed = user.EmailIsConfirmed
            };
        }
    }
}
