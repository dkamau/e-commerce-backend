using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using ECommerceBackend.Core.Entities.OrderEntities;
using ECommerceBackend.Core.Exceptions;
using ECommerceBackend.Core.Interfaces;
using ECommerceBackend.Web.ApiErrors;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ECommerceBackend.Web.Endpoints.OrderEndpoints
{
    [Route("/Orders")]
    public class Create : BaseAsyncEndpoint<OrderRequest, OrderResponse>
    {
        protected readonly ICrudService<Order> _orderCrudeService;
        protected readonly IOrderService _orderService;

        public Create(ICrudService<Order> orderCrudeService, IOrderService orderService)
        {
            _orderCrudeService = orderCrudeService;
            _orderService = orderService;
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Creates a new Order",
            Description = "Creates a new Order",
            OperationId = "Order.Create",
            Tags = new[] { "Order Endpoints" })
        ]
        public override async Task<ActionResult<OrderResponse>> HandleAsync(OrderRequest request, CancellationToken cancellationToken)
        {
            try
            {
                Order order = await _orderService.CreateAsync(new Order()
                {
                    UserId = request.UserId,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber,
                    Address1 = request.Address1,
                    Address2 = request.Address2,
                    SaveInfo = request.SaveInfo,
                    ZipCode = request.ZipCode,
                    Town = request.Town,
                    Country = request.Country,
                    ShippingIsSameAsBilling = request.ShippingIsSameAsBilling,
                    IsActive = true,
                }, request.Products);

                return Ok(OrderResponse.Create(order));
            }
            catch (CustomException ex)
            {
                return new CustomErrorResuslt().Error(ex);
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestError(ex.Message, ExceptionCode.InternalServerError.ToString()));
            }
        }
    }
}
