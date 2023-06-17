using Application.Dtos.Categories;
using FluentValidation;

namespace Application.Validators.Categories;

public class CategoryInputDtoValidator : AbstractValidator<CategoryInputDto>
{
    public CategoryInputDtoValidator()
    {
        RuleFor(a => a.ParentCategoryId);
        RuleFor(a => a.Name).MaximumLength(100).NotEmpty();
    }
}