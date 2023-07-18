using Application.Dtos.OrderItems;
using Application.Dtos.Orders;
using Bogus;
using Domain.Entities;

namespace UnitTests.OrderService.ReserveOrderProduct;

public class ReserveOrderTestData : TestDataBase<ReserveOrderTestCase>
{
    protected override IEnumerable<ReserveOrderTestCase> GetTestData()
    {
        var faker = new Faker();
        var productId = faker.Random.Long(1);
        var productQuantity = faker.Random.Long(10, 100);
        var price = faker.Random.Decimal();
        var brandId = faker.Random.Long(1);
        var name = faker.Commerce.ProductName();
        yield return new ReserveOrderTestCase()
        {
            Description = "Reserve 5 products from stock",
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
                        Quantity = 5,
                        ProductId = productId,
                    },
                },
            },
            ExpectedResult = new Product()
            {
                Id = productId,
                Price = price,
                Quantity = productQuantity - 5,
                BrandId = brandId,
                Name = name,
            },
        };
    }
}