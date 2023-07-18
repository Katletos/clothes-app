using Application.Dtos.Sections;
using FluentValidation;

namespace Application.Validators.Sections;

public class SectionDtoValidator : AbstractValidator<SectionDto>
{
    public SectionDtoValidator()
    {
        RuleFor(r => r.Id).NotEmpty();
        RuleFor(r => r.Name).MaximumLength(100).NotEmpty();
    }
}