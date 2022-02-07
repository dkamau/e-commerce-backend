using ECommerceBackend.Core.Entities.AuthenticationEntities;

namespace ECommerceBackend.Web.Endpoints.AuthenticationEndpoints
{
    public class LoginResponse
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string PhotoUrl { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Token { get; set; }
        public bool EmailIsConfirmed { get; set; } = false;

        public static LoginResponse Create(User user, string token)
        {
            return new LoginResponse()
            {
                UserId = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhotoUrl = user.PhotoUrl,
                Token = token,
                EmailIsConfirmed = user.EmailIsConfirmed
            };
        }
    }
}
