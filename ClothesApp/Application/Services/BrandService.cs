using Application.Dtos;
using Application.Exceptions;
using Application.Repositories;
using AutoMapper;
using Domain.Entities;

namespace Application.Services;

public class BrandService : IBrandService
{
    private readonly IBrandsRepository _brandsRepository;

    private readonly IProductRepository _productRepository;

    private readonly IMapper _mapper;

    public BrandService(IBrandsRepository brandsRepository, IProductRepository productRepository, IMapper mapper)
    {
        _brandsRepository = brandsRepository;
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<BrandDto> AddBrandAsync(CreateBrandDto createBrandDto)
    {
        var brandExists = await _brandsRepository.DoesBrandExistAsync(createBrandDto.Name);

        if (brandExists)
        {
            throw new DatabaseConflictException(Messages.AlreadyExistsConflict);
        }

        var brand = _mapper.Map<Brand>(createBrandDto);
        await _brandsRepository.InsertAsync(brand);
        var brandDto = _mapper.Map<BrandDto>(brand);

        return brandDto;
    }

    public async Task<BrandDto> UpdateBrandAsync(BrandDto brandDto)
    {
        var brand = _mapper.Map<Brand>(brandDto);

        var result = await _brandsRepository.GetBrandByIdAsync(brand.Id);

        if (result is null)
        {
            throw new NotFoundException(Messages.NotFound);
        }

        await _brandsRepository.UpdateAsync(brand);

        return brandDto;
    }

    public async Task<BrandDto> DeleteBrandByIdAsync(long id)
    {
        var isExist = await _brandsRepository.DoesBrandExistAsync(id);

        if (!isExist)
        {
            throw new NotFoundException();
        }

        var areAnyProducts = await _productRepository.AnyProductOfBrandIdExists(id);

        if (areAnyProducts)
        {
            throw new DatabaseConflictException(Messages.HasProductsConflict);
        }

        var brand = await _brandsRepository.GetBrandByIdAsync(id);
        var brandDto = _mapper.Map<BrandDto>(brand);
        await _brandsRepository.DeleteBrandByIdAsync(id);

        return brandDto;
    }

    public async Task<IReadOnlyCollection<BrandDto>> GetAllBrandsAsync()
    {
        var brands = await _brandsRepository.GetAllAsync();

        var brandsDto = _mapper.Map<IReadOnlyCollection<BrandDto>>(brands);

        return brandsDto;
    }

    public async Task<BrandDto> GetBrandByIdAsync(long id)
    {
        var brand = await _brandsRepository.GetBrandByIdAsync(id);

        if (brand is null)
        {
            throw new NotFoundException(Messages.NotFound);
        }

        var brandDto = _mapper.Map<BrandDto>(brand);

        return brandDto;
    }
}