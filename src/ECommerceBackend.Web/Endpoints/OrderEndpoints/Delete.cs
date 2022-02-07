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
    public class Delete : BaseAsyncEndpoint<int, OrderResponse>
    {
        private readonly ICrudService<Order> _orderService;

        public Delete(ICrudService<Order> orderService)
        {
            _orderService = orderService;
        }

        [HttpDelete("{orderId:int}")]
        [SwaggerOperation(
            Summary = "Deletes a Order",
            Description = "Deletes a Order",
            OperationId = "Order.Delete",
            Tags = new[] { "Order Endpoints" })
        ]
        public override async Task<ActionResult<OrderResponse>> HandleAsync(int orderId, CancellationToken cancellationToken)
        {
            try
            {
                await _orderService.DeleteAsync(orderId);
                return NoContent();
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
