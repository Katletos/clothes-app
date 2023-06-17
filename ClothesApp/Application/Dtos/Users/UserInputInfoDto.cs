using Domain.Enums;

namespace Application.Dtos.Users;

public class UserInputInfoDto
{
    public string Email { get; set; }
    
    public string Phone { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }
}