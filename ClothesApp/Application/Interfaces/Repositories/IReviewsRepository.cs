using System.Linq.Expressions;
using Application.Dtos.Reviews;
using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IReviewsRepository : IBaseRepository<Review>
{
    Task<Review> Delete(Review review);

    Task<bool> CanAdd(long productId, ReviewInputDto reviewInputDto);
    
    Task<IList<Review>> FindByCondition(Expression<Func<Review, bool>> expression);
}