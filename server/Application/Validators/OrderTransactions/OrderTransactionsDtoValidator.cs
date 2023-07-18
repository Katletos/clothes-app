using Application.Dtos.OrderTransactions;
using FluentValidation;

namespace Application.Validators.OrderTransactions;

public class OrderTransactionsDtoValidator : AbstractValidator<OrderTransactionsDto>
{
    public OrderTransactionsDtoValidator()
    {
        RuleFor(o => o.Id).NotEmpty();
        RuleFor(o => o.OrderId).NotEmpty();
        RuleFor(o => o.OrderStatus).NotEmpty();
        RuleFor(o => o.UpdatedAt).NotEmpty();
    }
}