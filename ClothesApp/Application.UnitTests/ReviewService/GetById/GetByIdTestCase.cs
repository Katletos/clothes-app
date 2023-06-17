using Application.Dtos.Reviews;
using Domain.Entities;

namespace UnitTests.ReviewService.GetById;

public class GetByIdTestCase
{
    public string Description { get; set; }
    
    public long Id { get; set; }

    public ReviewDto ExpectedResult { get; set; }

    public Review ReviewFromRepository { get; set; }
}