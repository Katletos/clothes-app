using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IMediaRepository
{
    Task Insert(Media media);

    Task<long[]> GetImageIdsByProductId(long id);

    Task<Media> GetById(long id);
}