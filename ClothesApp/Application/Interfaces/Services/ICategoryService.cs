using Application.Dtos.Category;

namespace Application.Interfaces.Services;

public interface ICategoryService
{
    Task<CategoryDto> GetById(long id);

    Task<CategoryDto> Add(CategoryInputDto categoryInputDto);

    Task<CategoryDto> Update(long id, CategoryInputDto categoryInputDto);

    Task<CategoryDto> DeleteById(long id);

    Task LinkCategoryToSection(long categoryId, long sectionId);
}