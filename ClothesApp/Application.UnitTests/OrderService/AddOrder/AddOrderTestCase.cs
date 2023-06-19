using Application.Dtos.Addresses;
using Application.Dtos.Orders;
using Domain.Entities;

namespace UnitTests.OrderService.AddOrder;

public class AddOrderTestCase
{
    public string Description { get; set; }
    
    public Product Product { get; set; }
    
    public OrderInputDto OrderInputDto { get; set; }

    public OrderDto OrderDto { get; set; }
    
    public Address Address { get; set; }
    
    public User User { get; set; }
}