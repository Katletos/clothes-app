using Application.Dtos.Category;
using Application.Dtos.SectionCategories;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Entities;

namespace Application.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    private readonly ISectionRepository _sectionRepository;

    private readonly ISectionCategoryRepository _sectionCategoryRepository;

    private readonly IMapper _mapper;

    public CategoryService(ICategoryRepository categoryRepository, IMapper mapper, ISectionRepository sectionRepository, ISectionCategoryRepository sectionCategoryRepository)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
        _sectionRepository = sectionRepository;
        _sectionCategoryRepository = sectionCategoryRepository;
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

        exist = await _categoryRepository.DoesExist(categoryInputDto.ParentCategoryId); 
        
        if (categoryInputDto.ParentCategoryId == 0 || exist)
        {
            var category = _mapper.Map<CategoryInputDto, Category>(categoryInputDto, opt => 
                opt.AfterMap((_, dest) => dest.ParentCategoryId = null));
            await _categoryRepository.Insert(category);
            var categoryDto = _mapper.Map<CategoryDto>(category);
            
            return categoryDto;
        }
        else
        {
            throw new BusinessRuleException(Messages.ParentCategoryConstraint);   
        }
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

        exist = await _categoryRepository.DoesExist(categoryInputDto.Name);

        if (exist)
        {
            throw new BusinessRuleException(Messages.CategoryUniqueConstraint);
        }
        
        if (category.ParentCategoryId == 0)
        {
            category.ParentCategoryId = null;
        }
        else
        {
            exist = await _categoryRepository.DoesExist(categoryInputDto.ParentCategoryId); 
        
            if (!exist)
            {
                throw new BusinessRuleException(Messages.ParentCategoryConstraint);
            }
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

    public async Task<SectionCategoryDto> LinkCategoryToSection(long categoryId, long sectionId)
    {
        var exist = await _categoryRepository.DoesExist(categoryId);

        if (!exist)
        {
            throw new NotFoundException(Messages.NotFound);
        }

        exist = await _sectionRepository.DoesExist(sectionId);

        if (!exist)
        {
            throw new NotFoundException(Messages.NotFound);
        }

        exist = await _sectionCategoryRepository.DoesExist(sectionId, categoryId );

        if (exist)
        {
            throw new BusinessRuleException();
        }
        
        var sectionCategory = new SectionCategory{ SectionId = sectionId, CategoryId = categoryId };
        await _sectionCategoryRepository.Add(sectionCategory);
        var sectionCategoryDto = _mapper.Map<SectionCategoryDto>(sectionCategory);
        
        return sectionCategoryDto;
    }
}