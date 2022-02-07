using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using ECommerceBackend.Core.Entities.AuthenticationEntities;
using ECommerceBackend.Core.Exceptions;
using ECommerceBackend.Core.Interfaces;
using ECommerceBackend.Core.Services;
using ECommerceBackend.Core.Specifications.UserSpecifications;
using ECommerceBackend.SharedKernel.Interfaces;
using ECommerceBackend.UnitTests.Core.Services.AuthenticationServiceTests.Factory;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace ECommerceBackend.UnitTests.Core.Services.AuthenticationServiceTests
{
    public class CreateUserAsync_Should
    {
        [Theory()]
        [InlineData(null)]
        [InlineData("")]
        public async Task Throw_CustomException_If_Password_NotProvided(string password)
        {
            // Arrange
            User user = GenerateNewUser();

            AuthenticationService userService = AuthenticationServiceFactory.Create(new Mock<IRepository>(), new Mock<IEmailService>(), new Mock<IConfiguration>());

            // Act
            // Assert
            var exception = await Assert.ThrowsAsync<CustomException>(() => userService.CreateUserAsync(user, password));
            Assert.Equal(ExceptionCode.NullReference.ToString(), exception.ExceptionCode);
        }

        [Fact]
        public async Task Throw_CustomException_If_Record_AlreadyExists()
        {
            // Arrange
            User user = GenerateNewUser();
            User existingUser = new User()
            {
                Id = 1,
                Email = user.Email,
            };

            Mock<IRepository> repository = new Mock<IRepository>();
            repository
                .Setup(n => n.FirstOrDefaultAsync(It.IsAny<GetUser>()))
                .Returns(Task.FromResult(existingUser));

            AuthenticationService userService = AuthenticationServiceFactory.Create(repository, new Mock<IEmailService>(), new Mock<IConfiguration>());

            // Act
            // Assert
            var exception = await Assert.ThrowsAsync<CustomException>(() => userService.CreateUserAsync(user, "some_randomn_password"));
            Assert.Equal(ExceptionCode.UserAlreadyExists.ToString(), exception.ExceptionCode);
        }

        [Fact]
        public async Task Generate_A_Valid_PasswordSalt()
        {
            // Arrange
            User user = GenerateNewUser();

            AuthenticationService userService = AuthenticationServiceFactory.Create(new Mock<IRepository>(), new Mock<IEmailService>(), new Mock<IConfiguration>());

            // Act
            var result = await userService.CreateUserAsync(user, "some_randomn_password");

            // Assert
            Assert.NotNull(result.PasswordSalt);
            Assert.Equal(128, result.PasswordSalt.Length);
        }

        [Fact]
        public async Task Generate_A_Valid_PasswordHash()
        {
            // Arrange
            User user = GenerateNewUser();

            AuthenticationService userService = AuthenticationServiceFactory.Create(new Mock<IRepository>(), new Mock<IEmailService>(), new Mock<IConfiguration>());

            // Act
            var result = await userService.CreateUserAsync(user, "some_randomn_password");

            // Assert
            Assert.NotNull(result.PasswordHash);
            Assert.Equal(64, result.PasswordHash.Length);
        }

        private User GenerateNewUser()
        {
            Fixture fixture = new Fixture();
            fixture.Customize(new AutoMoqCustomization());
            User user = fixture.Build<User>()
                .With(n => n.Id, 0).Create();

            return user;
        }
    }
}
