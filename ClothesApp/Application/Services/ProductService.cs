using Application.Dtos.OrderItems;
using Application.Dtos.Orders;
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
    
    public async Task<string> CheckProductsAvailability(long id)
    {
        var product = await _productsRepository.GetById(id);

        if (product is null)
        {
            throw new NotFoundException(Messages.NotFound);
        }
        
        if (product.Quantity > 0)
        {
            return Messages.ProductAvailable;
        }

        return Messages.ProductOutOfStock;
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
        var exists = await _brandsRepository.DoesExist(productInputDto.BrandId);

        if (!exists)
        {
            throw new NotFoundException(Messages.NotFound);
        }

        exists = await _categoryRepository.DoesExist(productInputDto.CategoryId);
        
        if (!exists)
        {
            throw new NotFoundException(Messages.NotFound);
        }

        exists = await _productsRepository.DoesExist(productInputDto.Name);
        
        if (!exists)
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

        var exists = await _productsRepository.DoesExist(id);

        if (!exists)
        {
            throw new NotFoundException(Messages.NotFound);
        }
        
        exists = await _brandsRepository.DoesExist(productInputDto.BrandId);

        if (!exists)
        {
            throw new NotFoundException(Messages.NotFound);
        }

        exists = await _categoryRepository.DoesExist(productInputDto.CategoryId);
        
        if (!exists)
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

    public async Task<ProductDto> AssignToBrand(long productId, long brandId)
    {
        var exist = await _brandsRepository.DoesExist(brandId);

        if (!exist)
        {
            throw new NotFoundException(Messages.NotFound);
        }

        exist = await _productsRepository.DoesExist(productId);

        if (!exist)
        {
            throw new NotFoundException(Messages.NotFound);
        }
     
        var product = await _productsRepository.GetById(productId);
        var productDto = _mapper.Map<Product, ProductDto>(product, opt =>
            opt.BeforeMap((src, _) => src.BrandId = brandId));
        
        await _productsRepository.Update(product);

        return productDto;
    }

    public async Task<ProductDto> UnassignFromBrand(long productId)
    {
        var product = await _productsRepository.GetById(productId);
        var productDto = _mapper.Map<Product, ProductDto>(product, opt =>
            opt.BeforeMap((src, _) => src.BrandId = null));
        
        await _productsRepository.Update(product);

        return productDto;
    }
}