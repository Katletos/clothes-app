using Application.Dtos.OrderItems;
using Application.Dtos.Orders;
using Bogus;
using Domain.Entities;
using Domain.Enums;

namespace UnitTests.OrderService.AddOrder;

public class AddOrderTestData : TestDataBase<AddOrderTestCase>
{
    protected override IEnumerable<AddOrderTestCase> GetTestData()
    {
        var faker = new Faker();
        var userId = faker.Random.Long(1);
        var productId = faker.Random.Long(1);
        var productQuantity = faker.Random.Long(1, 100);
        var productPrice = faker.Random.Long(1, 100);
        var orderPrice = productQuantity * productPrice;
        var addressId = faker.Random.Long(1);
        yield return new AddOrderTestCase()
        {
            Description = "Add order with one product",
            User = new User()
            {
                Id = userId,
                Email = faker.Internet.Email(),
                Password = faker.Internet.Password(),
                Phone = faker.Phone.PhoneNumber(),
                CreatedAt = DateTime.Now,
                UserType = faker.PickRandom<UserType>(),
                FirstName = faker.Name.FirstName(),
                LastName = faker.Name.LastName(),
            },
            Product = new Product()
            {
                Id = productId,
                Price = productPrice,
                Quantity = productQuantity,
                BrandId = faker.Random.Long(1),
                Name = faker.Commerce.ProductName(),
            },
            OrderDto = new OrderDto()
            {
                Id = 1,
                UserId = userId,
                Price = orderPrice,
                AddressId = addressId,
                OrderStatus = OrderStatusType.InReview,
            },
            OrderInputDto = new OrderInputDto()
            {
                UserId = userId,
                AddressId = addressId,
                OrderItems = new List<OrderItemInputDto>()
                {
                    new()
                    {
                        Quantity = productQuantity,
                        ProductId = productId,
                    }
                }
            },
            Address = new Address()
            {
                Id = addressId,
                UserId = userId,
                AddressLine = faker.Address.FullAddress(),
            }
        };
    }
}