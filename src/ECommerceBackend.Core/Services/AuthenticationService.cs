using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ECommerceBackend.Core.Entities.AuthenticationEntities;
using ECommerceBackend.Core.Exceptions;
using ECommerceBackend.Core.Helpers;
using ECommerceBackend.Core.Interfaces;
using ECommerceBackend.Core.Specifications.UserSpecifications;
using ECommerceBackend.Core.UserSecrets;
using ECommerceBackend.SharedKernel.Interfaces;
using Microsoft.Extensions.Configuration;

namespace ECommerceBackend.Core.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        protected readonly IRepository _repository;
        protected readonly IEmailService _emailService;
        protected readonly IConfiguration _configuration;

        public AuthenticationService(IRepository repository, IEmailService emailService, IConfiguration configuration)
        {
            _repository = repository;
            _emailService = emailService;
            _configuration = configuration;
        }

        public async Task<User> CreateUserAsync(User user, string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new CustomException(ExceptionCode.NullReference, "Please provide a password");

            if (await UserExistsAsync(user.Email))
                throw new CustomException(ExceptionCode.UserAlreadyExists, $"Email: {user.Email} has already been registered.");

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _repository.AddAsync(user);

            try
            {
                await SendAccountConfirmationEmail(user);
            }
            catch (Exception) { }

            return user;
        }

        public async Task<User> AuthenticateAsync(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return null;

            var user = await GetUserByEmail(email);

            if (user == null)
                return null;

            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            return user;
        }

        public async Task<User> ResendConfirmationLinkAsync(string email)
        {
            User user = await _repository.FirstOrDefaultAsync(new GetUser(email, null, null));

            if (user != null)
            {
                try
                {
                    await SendAccountConfirmationEmail(user);
                }
                catch (Exception ex)
                {
                    throw new CustomException(ExceptionCode.InternalServerError, ex.Message);
                }
            }

            return user;
        }

        public async Task<User> SendResetPasswordLinkAsync(string email)
        {
            User user = await _repository.FirstOrDefaultAsync(new GetUser(email, null, null));

            if (user != null)
            {
                try
                {
                    await SendResetPasswordEmail(user);
                }
                catch (Exception) { }
            }

            return user;
        }

        public async Task<User> ResetPasswordAsync(string encodedEmail, string password)
        {
            string email = StringHelper.Base64Decode(encodedEmail);
            User user = await _repository.FirstOrDefaultAsync(new GetUser(email, null, null));

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _repository.UpdateAsync(user);

            return user;
        }

        public async Task<User> ConfirmAccountAsync(string encodedEmail)
        {
            try
            {
                string email = StringHelper.Base64Decode(encodedEmail);

                User user = await _repository.FirstOrDefaultAsync(new GetUser(email, null, null));
                user.EmailIsConfirmed = true;
                await _repository.UpdateAsync(user);
                return user;
            }
            catch (Exception ex)
            {
                throw new CustomException(ExceptionCode.InternalServerError, ex.Message);
            }
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            User user = await GetUserByEmail(email);
            return user;
        }

        public async Task<bool> UserExistsAsync(string email)
        {
            User user = await GetUserByEmail(email);
            return user != null;
        }

        public async Task UpdateLoginLogs(User user)
        {
            LoginLog loginLog = new LoginLog()
            {
                UserId = user.Id,
            };

            await _repository.AddAsync(loginLog);
        }

        private async Task<User> GetUserByEmail(string email)
        {
            User user = await _repository.FirstOrDefaultAsync(new GetUser(email));
            return user;
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            if (string.IsNullOrWhiteSpace(password) || passwordHash.Length != 64 || passwordSalt.Length != 128)
                return false;

            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i])
                        return false;
                }
            }

            return true;
        }

        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        private async Task SendAccountConfirmationEmail(User user)
        {
            string frontEndLink = Environment.GetEnvironmentVariable("ECommerce_FRONTEND_LINK");
            if (string.IsNullOrEmpty(frontEndLink))
            {
                Links links = _configuration.GetSection("Links").Get<Links>();
                if (links != null)
                    frontEndLink = links.FrontEndApp;
            }

            string link = $"{frontEndLink}/authentication/confirmaccount/{StringHelper.Base64Encode(user.Email)}";
            string message = "We're excited to have you get started. First, you need to confirm your account. Just press the button below.";
            await SendEmail(user, "handshake", "Welcome to BET Shop", message, link, "Confirm Account");
        }

        private async Task SendResetPasswordEmail(User user)
        {
            string frontEndLink = Environment.GetEnvironmentVariable("ECommerce_FRONTEND_LINK");
            if (string.IsNullOrEmpty(frontEndLink))
            {
                Links links = _configuration.GetSection("Links").Get<Links>();
                if (links != null)
                    frontEndLink = links.FrontEndApp;
            }

            string link = $"{frontEndLink}/authentication/resetpassword/{StringHelper.Base64Encode(user.Email)}";
            string message = "We're are sorry you forgot your password. Don't worry, it happens. Just click the button below to setup a new one.";
            await SendEmail(user, "lock", "Reset Password", message, link, "Reset Password");
        }

        private async Task SendEmail(User user, string icon, string subject, string message, string link, string linkButtonText)
        {
            string email = File.ReadAllText("EmailTemplates/SingleLinkEmail.html")
               .Replace("[FirstName]", user.FirstName)
               .Replace("[Icon]", icon)
               .Replace("[Message]", message)
               .Replace("[Link]", link)
               .Replace("[LinkButtonText]", linkButtonText);

            await _emailService.SendEmailAsync($"{user.FirstName} {user.LastName} <{user.Email}>", subject, email);
        }
    }
}
