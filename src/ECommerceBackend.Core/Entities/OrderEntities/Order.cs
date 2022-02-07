using System;
using ECommerceBackend.SharedKernel;

namespace ECommerceBackend.Core.Entities.OrderEntities
{
    public class Order : BaseEntity
    {
        public int UserId { get; set; }
        public string OrderNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string ZipCode { get; set; }
        public string Town { get; set; }
        public string Country { get; set; }
        public bool ShippingIsSameAsBilling { get; set; }
        public bool SaveInfo { get; set; }
    }
}
