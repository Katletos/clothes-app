using Application.Dtos.Reviews;

namespace Application.Interfaces.Services;

public interface IReviewService
{
    Task<ReviewDto> Add(ReviewInputDto reviewInputDto);

    Task<ReviewDto> Update(long reviewId, ReviewInputDto reviewInputDto);

    Task<ReviewDto> DeleteById(long reviewId);

    Task<IList<ReviewDto>> GetByProductId(long productId);

    Task<IList<ReviewDto>> GetByUserId(long userId);

    Task<ReviewDto> GetById(long reviewId);
}