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

    public async Task Insert(Media media)
    {
        _dbContext.Media.Add(media);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<long[]> GetImageIdsByProductId(long id)
    {
        return await _dbContext.Media.Where(m => m.ProductId == id).Select(m => m.Id).ToArrayAsync();
    }

    public async Task<Media> GetById(long id)
    {
        return await _dbContext.Media.FindAsync(id);
    }
}