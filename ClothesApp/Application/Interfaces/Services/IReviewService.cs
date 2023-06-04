using Application.Dtos.Reviews;

namespace Application.Interfaces.Services;

public interface IReviewService
{
    Task<ReviewDto> Add(long id, ReviewInputDto reviewInputDto);

    Task<ReviewDto> Update(long id, ReviewInputDto reviewInputDto);

    Task<ReviewDto> DeleteById(long reviewId);

    Task<IList<ReviewDto>> GetByProductId(long id);

    Task<ReviewDto> GetById(long id);

    Task<IList<ReviewDto>> GetAll();
}