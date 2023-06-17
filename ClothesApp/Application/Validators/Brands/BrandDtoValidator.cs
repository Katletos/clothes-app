using Application.Dtos.Brands;
using FluentValidation;

namespace Application.Validators.Brands;

public class BrandDtoValidator : AbstractValidator<BrandDto>
{
    public BrandDtoValidator()
    {
        RuleFor(b => b.Id).NotEmpty();
        RuleFor(b => b.Name).MaximumLength(100).NotEmpty();
    }
}