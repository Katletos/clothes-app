using Domain.Enums;

namespace Application.Dtos.Users;

public struct RegisterUserDto
{
    public UserType UserType { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public string Phone { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }
}