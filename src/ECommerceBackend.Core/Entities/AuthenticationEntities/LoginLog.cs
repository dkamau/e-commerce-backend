using ECommerceBackend.SharedKernel;

namespace ECommerceBackend.Core.Entities.AuthenticationEntities
{
    public class LoginLog : BaseEntity
    {
        public int UserId { get; set; }
    }
}
