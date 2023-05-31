using Application.Dtos;
using FluentValidation;

namespace Application.Validators;

public class BrandDtoValidator : AbstractValidator<BrandDto>
{
    public BrandDtoValidator()
    {
        RuleFor(b => b.Id).NotEmpty();
        RuleFor(b => b.Name).MaximumLength(100).NotEmpty();
    }
}