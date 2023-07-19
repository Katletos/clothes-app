using Application.Dtos.Addresses;
using FluentValidation;

namespace Application.Validators.Addresses;

public class AddAddressDtoValidator : AbstractValidator<AddAddressDto>
{
    public AddAddressDtoValidator()
    {
        RuleFor(a => a.UserId).NotEmpty();
        RuleFor(a => a.AddressLine).MaximumLength(100).NotEmpty();
    }
}