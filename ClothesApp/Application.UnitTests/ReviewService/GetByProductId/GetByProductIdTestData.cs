using Application.Dtos.Reviews;
using Bogus;
using Domain.Entities;
using Domain.Enums;

namespace UnitTests.ReviewService.GetByProductId;

public class GetByProductIdTestData : TestDataBase<GetByProductIdTestCase>
{
    protected override IEnumerable<GetByProductIdTestCase> GetTestData()
    { 
        var faker = new Faker();
        var productId = faker.Random.Long(1);
        var userId = faker.Random.Long(1);
        var id = faker.Random.Long(1);
        var comment = faker.Commerce.ProductDescription();
        var rating = faker.Random.Short(1, 10);
        var title = faker.Commerce.ProductName();
        var createdAt = DateTime.Now;
        
        yield return new GetByProductIdTestCase()
        {
            ProductId = productId,
            Description = "The case of getting a review by product id",
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
            ReviewFromRepository = new List<Review>() {new()
            {
                Id = id,
                UserId = userId,
                ProductId = productId,
                Comment = comment,
                Rating = rating,
                Title = title,
                CreatedAt = createdAt,
            }},
            ExpectedResult = new List<ReviewDto>() {new()
            {
                Id = id,
                UserId = userId,
                ProductId = productId,
                Comment = comment,
                Rating = rating,
                Title = title,
                CreatedAt = createdAt,
            }}
        };
    }
}