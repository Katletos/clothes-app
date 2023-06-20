using Application.Dtos.Reviews;
using Domain.Entities;

namespace UnitTests.ReviewService.GetByProductId;

public class GetByProductIdTestCase
{
    public string Description { get; set; }
    
    public long ProductId { get; set; }
    
    public User User { get; set; }

    public List<ReviewDto> ExpectedResult { get; set; }

    public List<Review> ReviewFromRepository { get; set; }
}