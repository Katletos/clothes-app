using Application;
using Application.Dtos.OrderItems;
using Application.Dtos.Orders;
using Application.Dtos.OrderTransactions;
using Application.Exceptions;
using Application.Interfaces.Services;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Authentication;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/orders")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [Authorize]
    [HttpGet("{id}/history")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IList<OrderTransactionsDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<IList<OrderDto>>> GetOrderHistory([FromRoute] long id)
    {
        var userInfo = ClaimExtractor.GetUserInfo(User);
        var order = await _orderService.GetById(id);

        if (userInfo.UserType == UserType.Admin || order.UserId == userInfo.Id)
        {
            var orderHistory = await _orderService.GetOrderHistoryByOrderId(id);
            return Ok(orderHistory);
        }
        else
        {
            throw new BusinessRuleException(Messages.AuthorizationConstraint);
        }
    }

    [Authorize(Policy = Policies.Customer)]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<IList<OrderDto>>> CreateOrderForCustomer([FromBody] OrderInputDto orderInputDto)
    {
        var orderDto = await _orderService.Add(orderInputDto);

        return Ok(orderDto);
    }

    [Authorize(Policy = Policies.Admin)]
    [HttpPost("{id}/update-status")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<IList<OrderDto>>> ChangeOrderStatus([FromRoute] long id,
        [FromQuery] OrderStatusType newOrderStatus)
    {
        var orderDto = await _orderService.UpdateStatus(id, newOrderStatus);

        return Ok(orderDto);
    }

    [Authorize]
    [HttpGet("{id}/items")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IList<OrderItemDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<IList<OrderDto>>> GetOrderItemsForOrder([FromRoute] long id)
    {
        var userInfo = ClaimExtractor.GetUserInfo(User);
        var order = await _orderService.GetById(id);

        if (userInfo.UserType == UserType.Admin || order.UserId == userInfo.Id)
        {
            var orderItems = await _orderService.GetOrderItemsByOrderId(id);
            return Ok(orderItems);
        }
        else
        {
            throw new BusinessRuleException(Messages.AuthorizationConstraint);
        }
    }

    [Authorize(Policy = Policies.Admin)]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IList<OrderItemDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<IList<OrderDto>>> GetOrdersByStatus([FromQuery] OrderStatusType status)
    {
        var ordersDtos = await _orderService.GetOrdersByStatus(status);

        return Ok(ordersDtos);
    }
}