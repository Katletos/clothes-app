using Application.Dtos.Addresses;
using FluentValidation;

namespace Application.Validators.Addresses;

public class AddressDtoValidator : AbstractValidator<AddressDto>
{
    public AddressDtoValidator()
    {
        RuleFor(a => a.Id).NotEmpty();
        RuleFor(a => a.UserId).NotEmpty();
        RuleFor(a => a.AddressLine).MaximumLength(100).NotEmpty();
    }
}