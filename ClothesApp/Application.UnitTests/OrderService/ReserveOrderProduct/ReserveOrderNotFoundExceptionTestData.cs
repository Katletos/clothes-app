using Application.Dtos.OrderItems;
using Application.Dtos.Orders;
using Bogus;
using Domain.Entities;

namespace UnitTests.OrderService.ReserveOrderProduct;

public class ReserveOrderNotFoundExceptionTestData: TestDataBase<ReserveOrderNotFoundExceptionTestCase>
{
    protected override IEnumerable<ReserveOrderNotFoundExceptionTestCase> GetTestData()
    {
        var faker = new Faker();
        var productId = faker.Random.Long(1);
        var productQuantity = faker.Random.Long(10, 100);
        var price = faker.Random.Decimal();
        var brandId = faker.Random.Long(1);
        var name = faker.Commerce.ProductName();
        yield return new ReserveOrderNotFoundExceptionTestCase()
        {
            Description = "Reserve nonexistent product",
            Product = new Product()
            {
                Id = productId,
                Price = price,
                Quantity = productQuantity,
                BrandId = brandId,
                Name = name,
            },
            OrderInputDto = new OrderInputDto()
            {
                UserId = faker.Random.Long(1),
                AddressId = faker.Random.Long(1),
                OrderItems = new List<OrderItemInputDto>()
                {
                    new()
                    {
                        Quantity = productQuantity,
                        ProductId = productId - 1,
                    }
                }
            }
        };
    }
}