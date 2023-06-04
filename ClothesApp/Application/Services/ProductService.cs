using Application.Dtos.Products;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Application.Services;

public class ProductService : IProductService
{
    private readonly IProductsRepository _productsRepository;

    private readonly IBrandsRepository _brandsRepository;
    
    private readonly ICategoryRepository _categoryRepository;
    
    private readonly IMapper _mapper;

    public ProductService(IProductsRepository productsRepository, IMapper mapper, IBrandsRepository brandsRepository, ICategoryRepository categoryRepository)
    {
        _productsRepository = productsRepository;
        _mapper = mapper;
        _brandsRepository = brandsRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<IList<ProductDto>> GetAll()
    {
        var products = await _productsRepository.GetAll();
        var productsDto = _mapper.Map<IList<ProductDto>>(products);

        return productsDto;
    }

    public Task<ProductDto> GetById(long id)
    {
        throw new NotImplementedException();
    }

    public async Task<ProductDto> Add(ProductInputDto productInputDto)
    {
        var exist = await _brandsRepository.DoesExist(productInputDto.BrandId);

        if (!exist)
        {
            throw new NotFoundException(Messages.NotFound);
        }

        exist = await _categoryRepository.DoesExist(productInputDto.CategoryId);
        
        if (!exist)
        {
            throw new NotFoundException(Messages.NotFound);
        }

        exist = await _productsRepository.DoesExist(productInputDto.Name);
        
        if (!exist)
        {
            throw new BusinessRuleException(Messages.ProductNameUniqueConstraint);
        }
        
        _productsRepository.Insert();
    }

    public Task<ProductDto> Update(long id, ProductInputDto productInputDto)
    {
        throw new NotImplementedException();
    }

    public Task<ProductDto> DeleteById(long id)
    {
        throw new NotImplementedException();
    }

    public Task<IList<ProductDto>> GetProductsBySectionAndCategory(long sectionId, long categoryId)
    {
        throw new NotImplementedException();
    }

    public async Task<IList<ProductDto>> GetProductsByBrandId(long brandId)
    {
        var products = await _productsRepository.FindByCondition(p => p.BrandId == brandId);
        var productsDto = _mapper.Map<IList<ProductDto>>(products);

        return productsDto;
    }

    public Task AssignToBrand(long productId, long brandId)
    {
        throw new NotImplementedException();
    }

    public Task UnAssignFromBrand(long productId)
    {
        throw new NotImplementedException();
    }
}