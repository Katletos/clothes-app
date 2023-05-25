using ClothesApp.Dtos;
using ClothesApp.Entities;
using FluentValidation;

namespace ClothesApp.Validators;

public class CreateBrandDtoValidator : AbstractValidator<CreateBrandDto> 
{
    public CreateBrandDtoValidator()
    {
        RuleFor(b => b.Name).MaximumLength(100).NotEmpty();
    }
}
