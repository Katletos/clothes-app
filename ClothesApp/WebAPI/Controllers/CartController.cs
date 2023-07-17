using Application;
using Application.Dtos.CartItem;
using Application.Exceptions;
using Application.Interfaces.Services;
using Domain.Entities;
using Domain.Enums;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Authentication;
using WebAPI.Services;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/cart")]
public class CartController : ControllerBase
{
    private readonly ICartItemsService _cartItemsService;

    private readonly ISignalRService _signalRService;

    public CartController(ICartItemsService cartItemsService, ISignalRService signalRService)
    {
        _cartItemsService = cartItemsService;
        _signalRService = signalRService;
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

    // [Authorize]
    // [HttpPost("{userId}")]
    // [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CartItemDto))]
    // [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    // public async Task<ActionResult> AddItem([FromBody] CartItemDto cartItemDto, long userId)
    // {
    //     if (User.GetUserType() == UserType.Admin || userId == User.GetUserId())
    //     {
    //         var cartItem = await _cartItemsService.Add(cartItemDto);
    //
    //         await _signalRService.UpdateReservedQuantity(cartItemDto.ProductId);
    //         await _signalRService.UpdateAvailableQuantity(cartItemDto.ProductId);
    //         await _signalRService.UpdateCart(cartItemDto.UserId);
    //
    //         BackgroundJob.Schedule(() => _signalRService.DeleteExpiredCartItem(cartItemDto), TimeSpan.FromSeconds(20));
    //         
    //         return Ok(cartItem);
    //     }
    //     else
    //     {
    //         throw new BusinessRuleException(Messages.AuthorizationConstraint);
    //     }
    // }

    [Authorize]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CartItemDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<ActionResult> IncItemQuantity([FromBody] CartItemDto cartItemDto)
    {
        if (User.GetUserType() == UserType.Admin || cartItemDto.UserId == User.GetUserId())
        {
            var cartItem = await _cartItemsService.Add(cartItemDto);

            await _signalRService.UpdateReservedQuantity(cartItemDto.ProductId);
            await _signalRService.UpdateAvailableQuantity(cartItemDto.ProductId);
            await _signalRService.UpdateCart(cartItemDto.UserId);

            BackgroundJob.Schedule(() => _signalRService.DeleteExpiredCartItem(cartItemDto), TimeSpan.FromSeconds(20));
            return Ok(cartItem);
        }
        else
        {
            throw new BusinessRuleException(Messages.AuthorizationConstraint);
        }
    }

    [Authorize]
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CartItemDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<ActionResult> DecItemQuantity([FromBody] CartItemDto cartItemDto)
    {
        if (User.GetUserType() == UserType.Admin || cartItemDto.UserId == User.GetUserId())
        {
            var cartItem = await _cartItemsService.Remove(cartItemDto);

            await _signalRService.UpdateReservedQuantity(cartItemDto.ProductId);
            await _signalRService.UpdateAvailableQuantity(cartItemDto.ProductId);
            await _signalRService.UpdateCart(cartItemDto.UserId);

            return Ok(cartItem);
        }
        else
        {
            throw new BusinessRuleException(Messages.AuthorizationConstraint);
        }
    }

    [Authorize]
    [HttpDelete]
    public async Task<ActionResult> DropItemFromCart([FromBody] CartItemDto cartItemDto)
    {
        if (User.GetUserType() == UserType.Admin || cartItemDto.UserId == User.GetUserId())
        {
            var cartItem = await _cartItemsService.Drop(cartItemDto);

            await _signalRService.UpdateReservedQuantity(cartItemDto.ProductId);
            await _signalRService.UpdateAvailableQuantity(cartItemDto.ProductId);
            await _signalRService.UpdateCart(cartItemDto.UserId);

            return Ok(cartItem);
        }
        else
        {
            throw new BusinessRuleException(Messages.AuthorizationConstraint);
        }
    }
}