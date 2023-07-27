using Application;
using Application.Dtos.CartItem;
using Application.Exceptions;
using Application.Interfaces.Services;
using Domain.Entities;
using Domain.Enums;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebAPI.Authentication;
using WebAPI.Options;
using WebAPI.Services;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/cart")]
public class CartController : ControllerBase
{
    private readonly ICartItemsService _cartItemsService;

    private readonly IRealTimeService _realTimeService;

    private readonly ReservationOptions _reservationOptions;

    public CartController(ICartItemsService cartItemsService, IRealTimeService realTimeService,
        IOptions<ReservationOptions> reservationOptions)
    {
        _cartItemsService = cartItemsService;
        _realTimeService = realTimeService;
        _reservationOptions = reservationOptions.Value;
    }

    [Authorize]
    [HttpGet("{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IList<CartItem>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<ActionResult> GetUserCartItems([FromRoute] long userId)
    {
        if (User.GetUserType() == UserType.Admin || userId == User.GetUserId())
        {
            var cartItems = await _cartItemsService.GetUserCartItems(userId);
            return Ok(cartItems);
        }
        else
        {
            throw new BusinessRuleException(Messages.AuthorizationConstraint);
        }
    }

    [Authorize]
    [HttpPost("{userId}/items")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CartItemDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<ActionResult> AddItem([FromQuery] long productId, [FromRoute] long userId)
    {
        if (User.GetUserType() == UserType.Admin || userId == User.GetUserId())
        {
            var cartItem = await _cartItemsService.Add(productId, userId);

            await _realTimeService.UpdateReservedQuantity(productId);
            await _realTimeService.UpdateAvailableQuantity(productId);
            await _realTimeService.UpdateCart(userId);

            BackgroundJob.Schedule(
                () => _realTimeService.DeleteExpiredCartItem(productId, userId),
                TimeSpan.FromMinutes(_reservationOptions.ReservationTimeMinutes));

            return Ok(cartItem);
        }
        else
        {
            throw new BusinessRuleException(Messages.AuthorizationConstraint);
        }
    }

    [Authorize]
    [HttpPut("{userId}/items/{productId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CartItemDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<ActionResult> UpdateItemQuantity(
        [FromRoute] long userId,
        [FromRoute] long productId,
        [FromQuery] long newQuantity)
    {
        if (User.GetUserType() == UserType.Admin || userId == User.GetUserId())
        {
            var cartItem = await _cartItemsService.Update(productId, userId, newQuantity);

            await _realTimeService.UpdateReservedQuantity(productId);
            await _realTimeService.UpdateAvailableQuantity(productId);
            await _realTimeService.UpdateCart(userId);

            return Ok(cartItem);
        }
        else
        {
            throw new BusinessRuleException(Messages.AuthorizationConstraint);
        }
    }

    [Authorize]
    [HttpDelete("{userId}/items/{productId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CartItemDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<ActionResult> DeleteItem([FromRoute] long productId, [FromRoute] long userId)
    {
        if (User.GetUserType() == UserType.Admin || userId == User.GetUserId())
        {
            var cartItem = await _cartItemsService.Delete(productId, userId);

            await _realTimeService.UpdateReservedQuantity(productId);
            await _realTimeService.UpdateAvailableQuantity(productId);
            await _realTimeService.UpdateCart(userId);

            return Ok(cartItem);
        }
        else
        {
            throw new BusinessRuleException(Messages.AuthorizationConstraint);
        }
    }
}