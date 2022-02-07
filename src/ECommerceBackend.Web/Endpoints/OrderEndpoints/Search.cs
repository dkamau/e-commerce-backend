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
    [Route("/Orders")]
    public class Search : BaseAsyncEndpoint<SearchOrderRequest, OrderResponse>
    {
        private readonly ICrudService<Order> _orderService;

        public Search(ICrudService<Order> orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Get's a search of Orders",
            Description = "Get's a search of Orders",
            OperationId = "Order.Search",
            Tags = new[] { "Order Endpoints" })
        ]
        public override async Task<ActionResult<OrderResponse>> HandleAsync([FromQuery] SearchOrderRequest request, CancellationToken cancellationToken)
        {
            try
            {
                OrderFilter orderFilter = new OrderFilter()
                {
                    Search = request.Search,
                    OrderNumber = request.OrderNumber,
                };
                int totalRecords = await _orderService.CountAsync(orderFilter);

                orderFilter.Page = request.Page;
                orderFilter.RecordsPerPage = request.RecordsPerPage;
                List<Order> orders = await _orderService.ListAsync(orderFilter);


                if (request.Paginate)
                    return Ok(new ListOrderResponse(orders.OrderBy(n => n.OrderNumber).ToList(), request.Page, request.RecordsPerPage, totalRecords));
                else
                    return Ok(orders.OrderBy(n => n.OrderNumber).ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestError(ex.Message, ExceptionCode.InternalServerError.ToString()));
            }
        }
    }
}
