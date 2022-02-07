using System.Threading.Tasks;

namespace ECommerceBackend.Core.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body);
    }
}
