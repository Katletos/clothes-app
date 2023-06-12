using System.Linq.Expressions;
using Application.Dtos.Media;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class MediaRepository : IMediaRepository
{
    private readonly ClothesAppContext _dbContext;

    public MediaRepository(ClothesAppContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IList<Media>> FindByCondition(Expression<Func<Media, bool>> expression)
    {
        return await _dbContext.Set<Media>().Where(expression).ToListAsync();
    }

    public async Task Insert(Media media)
    {
        _dbContext.Media.Add(media);
        await _dbContext.SaveChangesAsync();
    }
}