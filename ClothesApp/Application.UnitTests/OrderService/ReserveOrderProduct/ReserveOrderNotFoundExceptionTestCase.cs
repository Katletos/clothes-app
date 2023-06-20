using Application.Dtos.Orders;
using Domain.Entities;

namespace UnitTests.OrderService.ReserveOrderProduct;

public class ReserveOrderNotFoundExceptionTestCase
{
    public string Description { get; set; }

    public Product Product { get; set; }

    public OrderInputDto OrderInputDto { get; set; }
}