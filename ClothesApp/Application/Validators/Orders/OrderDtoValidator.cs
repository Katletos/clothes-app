using Application.Dtos.Orders;
using FluentValidation;

namespace Application.Validators.Orders;

public class OrderDtoValidator : AbstractValidator<OrderDto>
{
    public OrderDtoValidator()
    {
        RuleFor(o => o.Id).NotEmpty();
        RuleFor(o => o.Price).NotEmpty();
        RuleFor(o => o.AddressId).NotEmpty();
        RuleFor(o => o.UserId).NotEmpty();
        RuleFor(o => o.CreatedAt).NotEmpty();
        RuleFor(o => o.OrderStatus).NotEmpty();
    }
}