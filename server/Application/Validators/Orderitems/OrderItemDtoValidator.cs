using Application.Dtos.OrderItems;
using FluentValidation;

namespace Application.Validators.OrderItems;

public class OrderItemDtoValidator : AbstractValidator<OrderItemDto>
{
    public OrderItemDtoValidator()
    {
        RuleFor(o => o.Price).NotEmpty();
        RuleFor(o => o.Quantity).NotEmpty();
        RuleFor(o => o.OrderId).NotEmpty();
        RuleFor(o => o.ProductId).NotEmpty();
    }
}