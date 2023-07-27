using Application.Dtos.Orders;
using Domain.Entities;

namespace UnitTests.OrderService.AddOrder;

public class AddOrderProductNotFoundTestCase
{
    public string Description { get; set; }

    public Product Product { get; set; }

    public OrderInputDto OrderInputDto { get; set; }

    public IList<CartItem> CartItems { get; set; }

    public OrderDto ExpectedResult { get; set; }

    public Address Address { get; set; }

    public User User { get; set; }
}