using Application.Dtos;
using FluentValidation;

namespace Application.Validators;

public class CreateBrandDtoValidator : AbstractValidator<CreateBrandDto>
{
    public CreateBrandDtoValidator()
    {
        RuleFor(b => b.Name).MaximumLength(100).NotEmpty();
    }
}