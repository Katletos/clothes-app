using ClothesApp.Data.Dtos;
using FluentValidation;

namespace ClothesApp.Data.Validators;

public class CreateBrandDtoValidator : AbstractValidator<CreateBrandDto> 
{
    public CreateBrandDtoValidator()
    {
        RuleFor(b => b.Name).MaximumLength(100).NotEmpty();
    }
}
