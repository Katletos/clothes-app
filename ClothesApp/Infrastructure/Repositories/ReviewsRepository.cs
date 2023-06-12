using System.Linq.Expressions;
using Application.Dtos.Reviews;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ReviewsRepository : IReviewsRepository
{
    private readonly ClothesAppContext _dbContext;

    public ReviewsRepository(ClothesAppContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Review> Insert(Review review)
    {
        _dbContext.Reviews.Add(review);
        await _dbContext.SaveChangesAsync();

        return review;
    }

    public async Task<Review> Update(Review review)
    {
        _dbContext.Update(review);
        await _dbContext.SaveChangesAsync();

        return review;
    }

    public async Task<IList<Review>> GetAll()
    {
        return await _dbContext.Reviews.AsNoTracking().ToListAsync();
    }

    public async Task<Review> GetById(long id)
    {
        return await _dbContext.Reviews.FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<bool> DoesExist(long id)
    {
        return await _dbContext.Reviews.AnyAsync(b => b.Id == id);
    }

    public async Task<Review> Delete(Review review)
    {
        _dbContext.Reviews.Remove(review);
        await _dbContext.SaveChangesAsync();
      
        return review;
    }

    public async Task<bool> DoesReviewExist(ReviewInputDto reviewInputDto)
    {
        return await _dbContext.Reviews.AnyAsync(r => r.ProductId == reviewInputDto.ProductId
                                                      && r.UserId == reviewInputDto.UserId);
    }

    public async Task<IList<Review>> FindByCondition(Expression<Func<Review, bool>> expression)
    {        
        return await _dbContext.Set<Review>().Where(expression).AsNoTracking().ToListAsync();
    }
}