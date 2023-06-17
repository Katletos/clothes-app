using Application.Dtos.Reviews;
using Domain.Entities;

namespace UnitTests.ReviewService.GetByProductId;

public class GetByProductIdTestData : TestDataBase<GetByProductIdTestCase>
{
    protected override IEnumerable<GetByProductIdTestCase> GetTestData()
    {
        yield return new GetByProductIdTestCase()
        {
            ProductId = 1,
            Description = "The case of getting a review by product id = 1",
            ReviewFromRepository = new List<Review>() {new()
            {
                Id = 1,
                UserId = 1,
                ProductId = 1,
                Comment = "Cool",
                Rating = 8,
                Title = "Cool",
                CreatedAt = DateTime.Now,
            }},
            ExpectedResult = new List<ReviewDto>() {new()
            {
                Id = 1,
                UserId = 1,
                ProductId = 1,
                Comment = "Cool",
                Rating = 8,
                Title = "Cool",
                CreatedAt = DateTime.Now,
            }}
        };
        yield return new GetByProductIdTestCase()
        {
            ProductId = 74,
            Description = "The case of getting a review by product id = 74",
            ReviewFromRepository = new List<Review>() {new()
            {
                Id = 89,
                UserId = 2,
                ProductId = 74,
                Comment = "Cool",
                Rating = 8,
                Title = "Cool",
                CreatedAt = DateTime.Now,
            }},
            ExpectedResult = new List<ReviewDto>() {new()
            {
                Id = 89,
                UserId = 2,
                ProductId = 74,
                Comment = "Nice",
                Rating = 8,
                Title = "Nice",
                CreatedAt = DateTime.Now,
            }}
        };
    }
}