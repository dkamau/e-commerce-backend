using ECommerceBackend.Core.Interfaces;
using ECommerceBackend.Core.Services;
using ECommerceBackend.SharedKernel.Interfaces;
using Microsoft.Extensions.Configuration;
using Moq;

namespace ECommerceBackend.UnitTests.Core.Services.AuthenticationServiceTests.Factory
{
    internal class AuthenticationServiceFactory
    {
        internal static AuthenticationService Create(Mock<IRepository> repository, Mock<IEmailService> emailService, Mock<IConfiguration> configuration)
        {
            return new AuthenticationService(repository.Object, emailService.Object, configuration.Object);
        }
    }
}
