using System;
using System.Linq;
using ECommerceBackend.Core.Entities.AuthenticationEntities;
using ECommerceBackend.Core.Services;
using ECommerceBackend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerceBackend.Web
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var dbContext = new AppDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>(), null))
            {
                // Look for any TODO items.
                PopulateTestData(dbContext);
            }
        }
        public static void PopulateTestData(AppDbContext dbContext)
        {
            if(!dbContext.Users.Any())
            {
                AuthenticationService authenticationService = new AuthenticationService(null, null, null);
                User user = new User()
                {
                    FirstName = "BET",
                    LastName = "Admin",
                    EmailIsConfirmed = true,
                    IsActive= true,
                    PhoneNumberIsConfirmed = true,
                    PhoneNumber = "(+254) 700 000000",
                    Email = "user@betsoftware.com",
                };

                byte[] passwordHash, passwordSalt;
                authenticationService.CreatePasswordHash("Bet@254!", out passwordHash, out passwordSalt);
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;

                dbContext.Add(user);
                dbContext.SaveChanges();
            }
        }
    }
}
