using Application.Dtos.Orders;
using FluentValidation;

namespace Application.Validators.Orders;

public class OrderInputDtoValidator : AbstractValidator<OrderInputDto>
{
    public OrderInputDtoValidator()
    {
        RuleFor(o => o.Price).NotEmpty();
        RuleFor(o => o.AddressId).NotEmpty();
        RuleFor(o => o.UserId).NotEmpty();
        RuleFor(o => o.CreatedAt).NotEmpty();
        RuleFor(o => o.OrderStatus).NotEmpty();
    }
}