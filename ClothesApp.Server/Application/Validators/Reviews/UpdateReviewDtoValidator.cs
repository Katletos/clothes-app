using Application.Dtos.Reviews;
using FluentValidation;

namespace Application.Validators.Reviews;

public class UpdateReviewDtoValidator : AbstractValidator<UpdateReviewDto>
{
    public UpdateReviewDtoValidator()
    {
        RuleFor(r => r.Comment).MaximumLength(1000).NotEmpty();
        RuleFor(r => r.Rating).InclusiveBetween((short)1, (short)10);
        RuleFor(r => r.Title).MaximumLength(100).NotEmpty();
    }
}