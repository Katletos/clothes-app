using Application.Dtos;
using FluentValidation;

namespace Application.Validators;

public class CreateBrandDtoValidator : AbstractValidator<BrandNameDto>
{
    public CreateBrandDtoValidator()
    {
        RuleFor(b => b.Name).MaximumLength(100).NotEmpty();
    }
}