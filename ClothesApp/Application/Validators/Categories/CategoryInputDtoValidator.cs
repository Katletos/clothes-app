using Application.Dtos.Category;
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