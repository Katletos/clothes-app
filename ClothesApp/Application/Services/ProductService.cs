using Application.Dtos.Products;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Entities;

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
    
    public Task<ProductDto> GetById(long id)
    {
        throw new NotImplementedException();
    }

    public async Task<ProductDto> Add(ProductInputDto productInputDto)
    {
        bool exist;
        if (productInputDto.BrandId is not null)
        {
            exist = await _brandsRepository.DoesExist((long)productInputDto.BrandId);

            if (!exist)
            {
                throw new NotFoundException(Messages.NotFound);
            }
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

        var product = _mapper.Map<Product>(productInputDto);
        var productDto = _mapper.Map<Product, ProductDto>(product, opt =>
            opt.BeforeMap((src, _) => src.CreatedAt = DateTime.Now));
        await _productsRepository.Insert(product);

        return productDto;
    }

    public async Task<ProductDto> Update(long id, ProductInputDto productInputDto)
    { 
        var product = _mapper.Map<ProductInputDto, Product>(productInputDto, opt => 
            opt.AfterMap((_, dest) => dest.Id = id));

        var exist = await _productsRepository.DoesExist(id);

        if (!exist)
        {
            throw new NotFoundException(Messages.NotFound);
        }

        if (productInputDto.BrandId is not null)
        {
            exist = await _brandsRepository.DoesExist((long)productInputDto.BrandId);

            if (!exist)
            {
                throw new NotFoundException(Messages.BrandNotFound);
            }    
        }

        exist = await _categoryRepository.DoesExist(productInputDto.CategoryId);
        
        if (!exist)
        {
            throw new NotFoundException(Messages.NotFound);
        }

        await _productsRepository.Update(product);

        var productDto = _mapper.Map<ProductDto>(product);
        
        return productDto;
    }

    public Task<ProductDto> DeleteById(long id)
    {
        throw new NotImplementedException();
    }

    public async Task<IList<ProductDto>> GetProductsBySectionAndCategory(long sectionId, long categoryId)
    {
        var products = await _productsRepository.FindByCondition(p => p.Category.Id == categoryId
                                                                          && p.Category.Sections.Any(s => s.Id == sectionId));
        var productsDto = _mapper.Map<IList<ProductDto>>(products);
        return productsDto;
    }

    public async Task<IList<ProductDto>> GetProductsByBrandId(long brandId)
    {
        var exist = await _brandsRepository.DoesExist(brandId);

        if (!exist)
        {
            throw new NotFoundException(Messages.NotFound);
        }
        
        var products = await _productsRepository.FindByCondition(p => p.BrandId == brandId);
        var productsDto = _mapper.Map<IList<ProductDto>>(products);

        return productsDto;
    }
}