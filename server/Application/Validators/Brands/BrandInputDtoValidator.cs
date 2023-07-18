using Application.Dtos.Brands;
using FluentValidation;

namespace Application.Validators.Brands;

public class BrandInputDtoValidator : AbstractValidator<BrandInputDto>
{
    public BrandInputDtoValidator()
    {
        RuleFor(b => b.Name).MaximumLength(100).NotEmpty();
    }
}