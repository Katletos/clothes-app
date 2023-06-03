using Application.Dtos.Products;
using FluentValidation;

namespace Application.Validators.Products;

public class ProductInputDtoValidator : AbstractValidator<ProductInputDto>
{
    public ProductInputDtoValidator()
    {
        RuleFor(o => o.Name).MaximumLength(100).NotEmpty();
        RuleFor(o => o.Price).NotEmpty();
        RuleFor(o => o.Quantity).NotEmpty();
        RuleFor(o => o.CategoryId).NotEmpty();
        RuleFor(o => o.BrandId).NotEmpty();
        RuleFor(o => o.CreatedAt).NotEmpty();
    }
}