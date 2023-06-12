using Application.Dtos.Addresses;
using Application.Dtos.Users;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/addresses")]
public class AddressController : ControllerBase
{
    private readonly IUserService _userService;

    public AddressController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPut("users/{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<ActionResult> UpdateUserAddress([FromRoute] long userId, [FromBody] AddressInputDto addressInputDto)
    {
        var userDto = await _userService.UpdateAddress(userId, addressInputDto);
        
        return Ok(userDto);
    }

    [HttpGet("users/{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IList<AddressDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<ActionResult> GetAddress([FromRoute] long userId)
    {
        var addressDto = await _userService.GetAddresses(userId);

        return Ok(addressDto);
    }

    [HttpDelete("{addressId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AddressDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<ActionResult> DeleteAddressById([FromRoute] long addressId)
    {
        var addressDto = await _userService.DeleteAddress(addressId);

        return Ok(addressDto);
    }
}