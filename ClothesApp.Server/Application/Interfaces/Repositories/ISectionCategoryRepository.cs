using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface ISectionCategoryRepository
{
    Task<bool> DoesExist(long sectionId, long categoryId);

    Task<bool> DoesSectionRelateCategory(long id);

    Task<SectionCategory> Add(SectionCategory sectionCategory);
}