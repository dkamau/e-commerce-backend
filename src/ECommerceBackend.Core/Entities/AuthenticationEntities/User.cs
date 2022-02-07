using ECommerceBackend.SharedKernel;

namespace ECommerceBackend.Core.Entities.AuthenticationEntities
{
    public class User : BaseEntity
    {
        public string PhotoUrl { get; set; }
        public string ImageFileId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool EmailIsConfirmed { get; set; } = false;
        public bool PhoneNumberIsConfirmed { get; set; } = false;
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
    }
}
