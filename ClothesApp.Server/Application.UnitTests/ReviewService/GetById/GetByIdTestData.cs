using Application.Dtos.Reviews;
using Bogus;
using Domain.Entities;

namespace UnitTests.ReviewService.GetById;

public class GetByIdTestData : TestDataBase<GetByIdTestCase>
{
    protected override IEnumerable<GetByIdTestCase> GetTestData()
    {
        var faker = new Faker();
        var id = faker.Random.Long();
        var userId = faker.Random.Long(1);
        var productId = faker.Random.Long(1);
        var comment = faker.Commerce.ProductDescription();
        var rating = faker.Random.Short(1, 10);
        var title = faker.Commerce.ProductName();
        var createdAt = DateTime.Now;

        yield return new GetByIdTestCase()
        {
            Description = "The case of getting an review by id = 1",
            Id = id,
            ReviewFromRepository = new Review
            {
                Id = id,
                UserId = userId,
                ProductId = productId,
                Comment = comment,
                Rating = rating,
                Title = title,
                CreatedAt = createdAt,
            },
            ExpectedResult = new ReviewDto
            {
                Id = id,
                UserId = userId,
                ProductId = productId,
                Comment = comment,
                Rating = rating,
                Title = title,
                CreatedAt = createdAt,
            },
        };
    }
}