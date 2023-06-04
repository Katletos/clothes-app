using Application.Dtos.Category;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Entities;

namespace Application.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    private readonly IMapper _mapper;

    public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }
    
    public async Task<CategoryDto> GetById(long id)
    {
        var category = await _categoryRepository.GetById(id);

        if (category is null)
        {
            throw new NotFoundException(Messages.NotFound);
        }

        var categoryDto = _mapper.Map<CategoryDto>(category);

        return categoryDto;
    }

    public async Task<CategoryDto> Add(CategoryInputDto categoryInputDto)
    {
        var exist = await _categoryRepository.DoesExist(categoryInputDto.Name);

        if (exist)
        {
            throw new BusinessRuleException(Messages.CategoryUniqueConstraint);
        }

        var category = _mapper.Map<Category>(categoryInputDto);
        await _categoryRepository.Insert(category);
        var categoryDto = _mapper.Map<CategoryDto>(category);

        return categoryDto;
    }

    public async Task<CategoryDto> Update(long id, CategoryInputDto categoryInputDto)
    {
        var category = _mapper.Map<CategoryInputDto, Category>(categoryInputDto, opt => 
            opt.AfterMap((_, dest) => dest.Id = id));

        var exist = await _categoryRepository.DoesExist(id);

        if (!exist)
        {
            throw new NotFoundException(Messages.NotFound);
        }

        await _categoryRepository.Update(category);

        var categoryDto = _mapper.Map<CategoryDto>(category);

        return categoryDto;
    }

    public async Task<CategoryDto> DeleteById(long id)
    {
        var exist = await _categoryRepository.DoesExist(id);

        if (!exist)
        {
            throw new NotFoundException(Messages.NotFound);
        }

        var parentCategory = await _categoryRepository.AreParentCategory(id);

        if (parentCategory)
        {
            throw new BusinessRuleException(Messages.CategoryDeleteConstraint);
        }

        var category = await _categoryRepository.GetById(id);
        await _categoryRepository.Delete(category);
        var categoryDto = _mapper.Map<CategoryDto>(category);

        return categoryDto;
    }

    public Task LinkCategoryToSection(long categoryId, long sectionId)
    {
        throw new NotImplementedException();
    }
}