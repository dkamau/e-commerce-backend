using Ardalis.Specification;
using ECommerceBackend.Core.Entities.AuthenticationEntities;

namespace ECommerceBackend.Core.Specifications.UserSpecifications
{
    public class GetUser : Specification<User>
    {
        public GetUser(string email = "", string userName = "", int? userId = null)
        {
            if (userId != null)
                Query.Where(n => n.Id == userId);
            if (!string.IsNullOrEmpty(email))
                Query.Where(n => n.Email == email.ToLower());
            if (!string.IsNullOrEmpty(userName))
                Query.Where(n => n.Username == userName);
        }
    }
}
