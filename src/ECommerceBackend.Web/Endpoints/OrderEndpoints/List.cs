using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using ECommerceBackend.Core.Entities.OrderEntities;
using ECommerceBackend.Core.Exceptions;
using ECommerceBackend.Core.Interfaces;
using ECommerceBackend.Core.Specifications.OrderSpecifications;
using ECommerceBackend.Web.ApiErrors;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ECommerceBackend.Web.Endpoints.OrderEndpoints
{
    [Route("/Users")]
    public class List : BaseAsyncEndpoint<ListOrderRequest, OrderResponse>
    {
        private readonly ICrudService<Order> _orderCrudService;
        protected readonly IOrderService _orderService;

        public List(ICrudService<Order> orderCrudService, IOrderService orderService)
        {
            _orderCrudService = orderCrudService;
            _orderService = orderService;
        }

        [FromRoute]
        public int userId { get; set; }

        [HttpGet("{userId:int}/Orders")]
        [SwaggerOperation(
            Summary = "Get's a list of Orders",
            Description = "Get's a list of Orders",
            OperationId = "Order.List",
            Tags = new[] { "Order Endpoints", "User Endpoints" })
        ]
        public override async Task<ActionResult<OrderResponse>> HandleAsync([FromQuery] ListOrderRequest request, CancellationToken cancellationToken)
        {
            try
            {
                OrderFilter orderFilter = new OrderFilter()
                {
                    UserId = userId,
                };
                int totalRecords = await _orderCrudService.CountAsync(orderFilter);

                orderFilter.Page = request.Page;
                orderFilter.RecordsPerPage = request.RecordsPerPage;
                List<Order> orders = await _orderCrudService.ListAsync(orderFilter);

                if (request.Paginate)
                    return Ok(new ListOrderResponse(orders.OrderByDescending(n => n.DateCreated).ToList(), request.Page, request.RecordsPerPage, totalRecords));
                else
                    return Ok(orders.Select(n => OrderResponse.Create(n)).OrderByDescending(n => n.DateCreated).ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestError(ex.Message, ExceptionCode.InternalServerError.ToString()));
            }
        }
    }
}
