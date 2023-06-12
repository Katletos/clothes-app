using Application.Exceptions;
using Domain.Enums;

namespace Application;

public class OrderStateMachine
{
    private OrderStatusType _state;

    public OrderStatusType CurrentState => _state;

    public OrderStateMachine(OrderStatusType state)
    {
        _state = state;
    }

    public void DoTransition(OrderStatusType newState)
    {
        _state = (_state, newState) switch
        {
            (OrderStatusType.InReview, OrderStatusType.InDelivery) => _state = OrderStatusType.InDelivery,
            (OrderStatusType.InReview, OrderStatusType.Cancelled) => _state = OrderStatusType.Cancelled,
            (OrderStatusType.InDelivery, OrderStatusType.Completed) => _state = OrderStatusType.Completed,
            (OrderStatusType.InDelivery, OrderStatusType.Cancelled) => _state = OrderStatusType.Cancelled,
            _ => throw new BusinessRuleException("Incorrect order status transition"),
        };
    }
}