using Application.Dtos.Users;
using Domain.Enums;
using FluentValidation;

namespace Application.Validators.Users;

public class UserInputDtoValidator : AbstractValidator<UserInputInfoDto>
{
    public UserInputDtoValidator()
    {
        RuleFor(u => u.Email).EmailAddress().NotEmpty();
        RuleFor(u => u.FirstName).MaximumLength(100).NotEmpty();
        RuleFor(u => u.LastName).MaximumLength(100).NotEmpty();
        RuleFor(u => u.Phone).MaximumLength(12).NotEmpty();
    }
}