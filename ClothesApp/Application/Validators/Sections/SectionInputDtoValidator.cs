using Application.Dtos.Sections;
using FluentValidation;

namespace Application.Validators.Sections;

public class SectionInputDtoValidator : AbstractValidator<SectionInputDto>
{
    public SectionInputDtoValidator()
    {
        RuleFor(r => r.Name).MaximumLength(100).NotEmpty();
    }
}