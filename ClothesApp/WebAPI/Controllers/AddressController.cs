using Application;
using Application.Dtos.Addresses;
using Application.Dtos.Users;
using Application.Exceptions;
using Application.Interfaces.Services;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Authentication;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/addresses")]
public class AddressController : ControllerBase
{
    private readonly IAddressService _addressService;

    public AddressController(IAddressService addressService)
    {
        _addressService = addressService;
    }

    [Authorize(Policy = Policies.Customer)]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AddressDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<ActionResult> AddUserAddress([FromBody] AddAddressDto addAddressDto)
    {
        var addressDto = await _addressService.AddAddress(addAddressDto);

        return Ok(addressDto);
    }

    [Authorize(Policy = Policies.Customer)]
    [HttpPut("users/{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<ActionResult> UpdateUserAddress([FromRoute] long userId,
        [FromBody] AddressInputDto addressInputDto)
    {
        var userDto = await _addressService.UpdateAddress(userId, addressInputDto);

        return Ok(userDto);
    }

    [Authorize]
    [HttpGet("users/{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IList<AddressDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<ActionResult> GetAddress([FromRoute] long userId)
    {
        if (User.GetUserType() == UserType.Admin || User.GetUserId() == userId)
        {
            var addressDto = await _addressService.GetAddresses(userId);
            return Ok(addressDto);
        }
        else
        {
            throw new BusinessRuleException(Messages.AuthorizationConstraint);
        }
    }

    [Authorize(Policy = Policies.Customer)]
    [HttpDelete("{addressId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AddressDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<ActionResult> DeleteAddressById([FromRoute] long addressId)
    {
        var addressDto = await _addressService.DeleteAddress(addressId);

        return Ok(addressDto);
    }
}