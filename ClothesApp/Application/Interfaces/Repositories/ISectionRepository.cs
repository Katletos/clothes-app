using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface ISectionRepository : IBaseRepository<Section>
{
    Task<bool> DoesExist(string name);

    Task<bool> AreSameName(long id, string name);

    Task<Section> Delete(Section section);
}