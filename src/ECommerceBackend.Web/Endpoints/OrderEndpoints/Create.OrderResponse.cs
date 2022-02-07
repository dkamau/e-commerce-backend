using System;
using System.Linq;
using ECommerceBackend.Core.Entities.OrderEntities;

namespace ECommerceBackend.Web.Endpoints.OrderEndpoints
{
    public class OrderResponse
    {
        public int Id { get; set; }
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
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public bool IsActive { get; set; } = true;
        public bool Deleted { get; set; } = false;

        public static OrderResponse Create(Order order)
        {
            OrderResponse orderResponse = new OrderResponse()
            {
                Id = order.Id,
                UserId = order.UserId,
                OrderNumber = order.OrderNumber,
                FirstName = order.FirstName,
                LastName = order.LastName,
                Email = order.Email,
                PhoneNumber = order.PhoneNumber,
                Address1 = order.Address1,
                Address2 = order.Address2,
                ZipCode = order.ZipCode,
                Town = order.Town,
                Country = order.Country,
                ShippingIsSameAsBilling = order.ShippingIsSameAsBilling,
                SaveInfo = order.SaveInfo,
                DateCreated = order.DateCreated,
                DateUpdated = order.DateUpdated,
                IsActive = order.IsActive,
                Deleted = order.Deleted
            };

            return orderResponse;
        }
    }
}
