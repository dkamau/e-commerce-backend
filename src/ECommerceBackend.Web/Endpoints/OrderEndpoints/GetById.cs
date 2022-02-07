using System;
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
    public class GetById : BaseAsyncEndpoint<int, OrderResponse>
    {
        protected readonly ICrudService<Order> _orderCrudService;
        protected readonly IOrderService _orderService;

        public GetById(ICrudService<Order> orderCrudService, IOrderService orderService)
        {
            _orderCrudService = orderCrudService;
            _orderService = orderService;
        }

        [HttpGet("{orderId:int}")]
        [SwaggerOperation(
            Summary = "Get's a single Order by Id",
            Description = "Get's a single Order by Id",
            OperationId = "Order.GetById",
            Tags = new[] { "Order Endpoints" })
        ]
        public override async Task<ActionResult<OrderResponse>> HandleAsync(int orderId, CancellationToken cancellationToken)
        {
            try
            {
                Order order = await _orderCrudService.GetByIdAsync(orderId);

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
