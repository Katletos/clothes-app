using Application.Dtos.Reviews;
using FluentValidation;

namespace Application.Validators.Reviews;

public class ReviewDtoValidator : AbstractValidator<ReviewDto>
{
    public ReviewDtoValidator()
    {
        RuleFor(r => r.Id).NotEmpty();
        RuleFor(r => r.ProductId).NotEmpty();
        RuleFor(r => r.UserId).NotEmpty();
        RuleFor(r => r.Comment).MaximumLength(1000).NotEmpty();
        RuleFor(r => r.Rating).InclusiveBetween((short)1, (short)10);
        RuleFor(r => r.Title).MaximumLength(100).NotEmpty();
        RuleFor(r => r.CreatedAt).NotEmpty();
    }
}