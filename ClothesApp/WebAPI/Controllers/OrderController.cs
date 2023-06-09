using Application.Dtos.OrderItems;
using Application.Dtos.Orders;
using Application.Dtos.OrderTransactions;
using Application.Interfaces.Services;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;

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
    
    [HttpGet("{id}/history")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IList<OrderTransactionsDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<IList<OrderDto>>> GetOrderHistory([FromRoute] long id)
    {
        var orderHistory = await _orderService.GetOrderHistoryByOrderId(id);

        return Ok(orderHistory);
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<IList<OrderDto>>> CreateOrderForCustomer([FromBody] OrderInputDto orderInputDto)
    {
        var orderDto = await _orderService.Add(orderInputDto);

        return Ok(orderDto);
    }
    
    [HttpPost("{id}/submit")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<IList<OrderDto>>> SubmitOrder([FromRoute] long id)
    {
        var orderDto = await _orderService.SubmitOrder(id);
        
        return Ok(orderDto);
    }
    
    [HttpPost("{id}/next-status")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<IList<OrderDto>>> ChangeOrderStatus([FromRoute] long id)
    {
        var orderDto = await _orderService.UpdateStatus(id);
        
        return Ok(orderDto);
    }
    
    [HttpGet("{id}/items")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IList<OrderItemDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<IList<OrderDto>>> GetOrderItemsForOrder([FromRoute] long id)
    {
        var orderItems = await _orderService.GetOrderItemsByOrderId(id);

        return Ok(orderItems);
    }
    
    [HttpPost("{id}/items")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderItemDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<IList<OrderDto>>> AddProductToOrder([FromRoute] long id, [FromBody] OrderItemInputDto orderItemInputDto)
    {
        var orderItemDto = await _orderService.AddItem(id, orderItemInputDto);

        return Ok(orderItemDto);
    }
}