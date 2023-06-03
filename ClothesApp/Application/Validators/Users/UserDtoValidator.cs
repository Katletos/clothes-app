using Application.Dtos.Users;
using Domain.Enums;
using FluentValidation;

namespace Application.Validators.Users;

public class UserDtoValidator : AbstractValidator<UserDto>
{
    public UserDtoValidator()
    {
        RuleFor(o => o.Id).NotEmpty();
        RuleFor(o => o.CreatedAt).NotEmpty();
        RuleFor(o => o.UserType).NotEmpty();
        RuleFor(o => o.Email).NotEmpty();
        RuleFor(o => o.Password).NotEmpty();
        RuleFor(o => o.Phone).NotEmpty().When(o => o.UserType == UserType.Customer);
        RuleFor(o => o.FirstName).NotEmpty().When(o => o.UserType == UserType.Customer);
        RuleFor(o => o.LastName).NotEmpty().When(o => o.UserType == UserType.Customer);
    }
}