using System.Linq.Expressions;
using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IMediaRepository
{
    Task<IList<Media>> FindByCondition(Expression<Func<Media, bool>> expression);

    Task Insert(Media media);
}