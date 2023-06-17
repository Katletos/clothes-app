using Application.Dtos.Reviews;
using Domain.Entities;

namespace UnitTests.ReviewService.GetById;

public class GetByIdTestData : TestDataBase<GetByIdTestCase>
{
    protected override IEnumerable<GetByIdTestCase> GetTestData()
    {
        yield return new GetByIdTestCase()
        {
            Description = "The case of getting an review by id = 1",
            Id = 1,
            ReviewFromRepository = new Review
            {
                Id = 1,
                UserId = 1,
                ProductId = 1,
                Comment = "Cool",
                Rating = 8,
                Title = "Cool",
                CreatedAt = DateTime.Now, 
            },
            ExpectedResult = new ReviewDto 
            {
                Id = 1,
                UserId = 1,
                ProductId = 1,
                Comment = "Cool",
                Rating = 8,
                Title = "Cool",
                CreatedAt = DateTime.Now,
            }
        };
        yield return new GetByIdTestCase()
        {
            Description = "The case of getting an review by id = 56",
            Id = 56,
            ReviewFromRepository = new Review
            {
                Id = 56,
                UserId = 9,
                ProductId = 10,
                Comment = "Awesome",
                Rating = 9,
                Title = "Awesome",
                CreatedAt = DateTime.Now, 
            },
            ExpectedResult = new ReviewDto 
            {
                Id = 56,
                UserId = 9,
                ProductId = 10,
                Comment = "Awesome",
                Rating = 9,
                Title = "Awesome",
                CreatedAt = DateTime.Now, 
            }
        };
    }
}