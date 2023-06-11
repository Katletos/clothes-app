using Application.Dtos.Addresses;
using Application.Dtos.Users;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IList<UserDto>))]
    public async Task<ActionResult<IList<UserDto>>> GetAllUsers()
    {
        var userDtos = await _userService.GetAll();

        return Ok(userDtos);
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<UserDto>> GetUserById([FromRoute] long id)
    {
        var userDto = await _userService.GetById(id);
        
        return Ok(userDto);
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<ActionResult> RegisterUser([FromBody] RegisterUserDto registerUserDto)
    {
        var userDto = await _userService.Add(registerUserDto);
        
        return Ok(userDto);
    }
    
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<ActionResult> UpdateUser([FromRoute] long id, [FromBody] UserInputInfoDto userInputInfoDto)
    {
        var userDto = await _userService.Update(id, userInputInfoDto);
        
        return Ok(userDto);
    }
    
    [HttpPut("{id}/addresses")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<ActionResult> UpdateUserAddress([FromRoute] long id, [FromBody] AddressInputDto addressInputDto)
    {
        var userDto = await _userService.UpdateAddress(id, addressInputDto);
        
        return Ok(userDto);
    }

    [HttpGet("{id}/addresses")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AddressDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<ActionResult> GetAddress([FromRoute] long id)
    {
        var addressDto = await _userService.GetAddresses(id);

        return Ok(addressDto);
    }

    [HttpDelete("addresses/{addressId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AddressDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<ActionResult> DeleteAddressById([FromRoute] long addressId)
    {
        var addressDto = await _userService.DeleteAddress(addressId);

        return Ok(addressDto);
    }

    [HttpPost("/login")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<ActionResult> LoginUser(UserLoginDto userLoginDto)
    {
        var login = await _userService.Login(userLoginDto);
        
        return Ok(login);
    }
}