using Application.Dtos.Users;
using FluentValidation;

namespace Application.Validators.Users;

public class UserLoginDtoValidator : AbstractValidator<UserLoginDto>
{
    public UserLoginDtoValidator()
    {
        RuleFor(o => o.Email).EmailAddress().NotEmpty();
        RuleFor(o => o.Password).MaximumLength(100).NotEmpty();
    }
}