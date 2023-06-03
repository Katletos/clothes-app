using Application.Dtos.Addresses;
using FluentValidation;

namespace Application.Validators.Addresses;

public class AddressInputDtoValidator : AbstractValidator<AddressInputDto>
{
    public AddressInputDtoValidator()
    {
        RuleFor(a => a.UserId).NotEmpty();
        RuleFor(a => a.AddressLine).MaximumLength(100).NotEmpty();
    }
}