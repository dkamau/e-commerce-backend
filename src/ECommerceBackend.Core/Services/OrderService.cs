using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerceBackend.Core.Constants;
using ECommerceBackend.Core.Entities.OrderEntities;
using ECommerceBackend.Core.Entities.ProductEntities;
using ECommerceBackend.Core.Exceptions;
using ECommerceBackend.Core.Interfaces;
using ECommerceBackend.Core.Specifications.OrderSpecifications;
using ECommerceBackend.Core.UserSecrets;
using ECommerceBackend.SharedKernel.Interfaces;
using Microsoft.Extensions.Configuration;

namespace ECommerceBackend.Core.Services
{
    public class OrderService : ICrudService<Order>, IOrderService
    {
        protected readonly IRepository _repository;
        protected readonly IEmailService _emailService;
        protected readonly IConfiguration _configuration;

        public OrderService(IRepository repository, IEmailService emailService, IConfiguration configuration)
        {
            _repository = repository;
            _emailService = emailService;
            _configuration = configuration;
        }

        public async Task<int> CountAsync(object filter)
        {
            return await _repository.CountAsync(new GetOrders((OrderFilter)filter));
        }

        public async Task<Order> CreateAsync(Order order)
        {
            return await _repository.AddAsync(order);
        }

        public async Task<Order> CreateAsync(Order order, List<Product> products)
        {
            order.OrderNumber = await GenerateOrderNumber();
            Order createdOrder = await CreateAsync(order);
            List<OrderedProduct> orderedProducts = new List<OrderedProduct>();

            foreach(var product in products.ToList())
            {
                orderedProducts.Add(new OrderedProduct()
                {
                    OrderId = createdOrder.Id,
                    ProductId = product.Id
                });
            }

            await _repository.AddRangeAsync(orderedProducts);

            await SendOrderEmail(order, products);

            return createdOrder;
        }

        public async Task SendOrderEmail(Order order, List<Product> products)
        {
            string frontEndLink = Environment.GetEnvironmentVariable("ECommerce_FRONTEND_LINK");
            if (string.IsNullOrEmpty(frontEndLink))
            {
                Links links = _configuration.GetSection("Links").Get<Links>();
                if (links != null)
                    frontEndLink = links.FrontEndApp;
            }

            StringBuilder stringBuilder = new StringBuilder();

            foreach (var product in products)
            {
                stringBuilder.Append(@$"<tr>
                    <td align='left' valign='top'>{product.Name}</td>
                    <td style='border-left:1px solid #000;' align='left' valign='top'>{product.Description}</td>
                    <td style='border-left:1px solid #000;' align='right' valign='top'>{product.SellingPrice}</td>
                  </tr>");
            }

            string productsHtml = File.ReadAllText("EmailTemplates/OrderedProducts.html")
                .Replace("[Products]", stringBuilder.ToString())
                .Replace("[TotalPrice]", string.Format("Kes. {0:N2}", products.Sum(n => n.SellingPrice)));


            await SendEmail(order, "shopping-cart--v2", $"Order: #{order.OrderNumber}", productsHtml, $"{frontEndLink}", "Continue Shopping");
        }

        private async Task SendEmail(Order order, string icon, string subject, string message, string link, string linkButtonText)
        {
            string email = File.ReadAllText("EmailTemplates/SingleLinkEmail.html")
               .Replace("[FirstName]", order.FirstName)
               .Replace("[Icon]", icon)
               .Replace("[Message]", message)
               .Replace("[Link]", link)
               .Replace("[LinkButtonText]", linkButtonText);

            await _emailService.SendEmailAsync($"{order.FirstName} {order.LastName} <{order.Email}>", subject, email);
        }

        public async Task DeleteAsync(int orderId)
        {
            Order order = await GetByIdAsync(orderId);
            await _repository.DeleteAsync(order);
        }

        public async Task<string> GenerateOrderNumber()
        {
            int currentOrderCount = await CountAsync(new OrderFilter()
            {
            });

            return string.Format("{0}", ModuleNumber.Order + currentOrderCount + 1);
        }

        public async Task<Order> GetByIdAsync(int orderId)
        {
            return await GetOrder(orderId);
        }

        public Task<Order> GetBySlugAsync(string slug)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Order>> ListAsync(object filter)
        {
            return await _repository.ListAsync(new GetOrders((OrderFilter)filter));
        }

        public async Task<bool> RecordExistsAsync(int orderId)
        {
            try
            {
                var order = await GetOrder(orderId);
                return order != null;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<Order> SoftDeleteAsync(int orderId)
        {
            Order order = await GetByIdAsync(orderId);

            order.Deleted = true;
            await _repository.UpdateAsync(order);

            return order;
        }

        public async Task<Order> UpdateAsync(Order order)
        {
            if (order.Id <= 0)
                throw new CustomException(ExceptionCode.InvalidOrderId);

            var existingOrder = await _repository.FirstOrDefaultAsync(new GetOrders(new OrderFilter()
            {
                Id = order.Id,
            }));

            if (existingOrder != null)
                if (order.Id != existingOrder.Id)
                    throw new CustomException(ExceptionCode.OrderAlreadyExists);

            order.DateUpdated = DateTime.UtcNow;
            await _repository.UpdateAsync(order);

            return order;
        }

        private async Task<Order> GetOrder(int orderId)
        {
            if (orderId <= 0)
                throw new CustomException(ExceptionCode.InvalidOrderId);

            Order order = await _repository.FirstOrDefaultAsync(new GetOrders(new OrderFilter()
            {
                Id = orderId,
            }));

            if (order == null)
                throw new CustomException(ExceptionCode.OrderNotFound);

            return order;
        }

        private async Task<bool> OrderExists(Order order)
        {
            Order existingOrder = await _repository.FirstOrDefaultAsync(new GetOrders(new OrderFilter()
            {
                OrderNumber = order.OrderNumber,
            }));

            if (existingOrder != null)
                throw new CustomException(ExceptionCode.OrderAlreadyExists);

            return false;
        }
    }
}
