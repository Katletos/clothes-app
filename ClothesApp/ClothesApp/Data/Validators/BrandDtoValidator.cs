using ClothesApp.Dtos;
using FluentValidation;

namespace ClothesApp.Validators;

public class BrandDtoValidator : AbstractValidator<BrandDto> 
{
    public BrandDtoValidator()
    {
        RuleFor(b => b.Id).NotEmpty();
        RuleFor(b => b.Name).MaximumLength(100).NotEmpty();
    }
}
