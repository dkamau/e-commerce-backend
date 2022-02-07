using System.Threading.Tasks;
using ECommerceBackend.Core.Entities.AuthenticationEntities;

namespace ECommerceBackend.Core.Interfaces
{
    public interface IAuthenticationService
    {
        Task<User> CreateUserAsync(User user, string password);
        Task<bool> UserExistsAsync(string email);
        Task<User> GetUserByEmailAsync(string email);
        Task<User> AuthenticateAsync(string email, string password);
        Task UpdateLoginLogs(User user);
        Task<User> ConfirmAccountAsync(string encodedEmail);
        Task<User> ResendConfirmationLinkAsync(string email);
        Task<User> SendResetPasswordLinkAsync(string email);
        Task<User> ResetPasswordAsync(string encodedEmail, string password);
    }
}
