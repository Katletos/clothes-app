using Application.Dtos.Category;
using FluentValidation;

namespace Application.Validators.Categories;

public class CategoryDtoValidator : AbstractValidator<CategoryDto>
{
    public CategoryDtoValidator()
    {
        RuleFor(a => a.Id).NotEmpty();
        RuleFor(a => a.ParentCategoryId).NotEmpty();
        RuleFor(a => a.Name).MaximumLength(100).NotEmpty();
    }
}