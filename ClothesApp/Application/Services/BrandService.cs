using Application.Dtos;
using Application.Exceptions;
using Application.Repositories;
using AutoMapper;
using Domain.Entities;

namespace Application.Services;

public class BrandService : IBrandService
{
    private readonly IBrandsRepository _brandsRepository;

    private readonly IProductsRepository _productsRepository;

    private readonly IMapper _mapper;

    public BrandService(IBrandsRepository brandsRepository, IProductsRepository productsRepository, IMapper mapper)
    {
        _brandsRepository = brandsRepository;
        _productsRepository = productsRepository;
        _mapper = mapper;
    }

    public async Task<BrandDto> AddBrand(CreateBrandDto createBrandDto)
    {
        var brandExists = await _brandsRepository.DoesBrandExist(createBrandDto.Name);

        if (brandExists)
        {
            throw new DatabaseConflictException(Messages.AlreadyExistsConflict);
        }

        var brand = _mapper.Map<Brand>(createBrandDto);
        await _brandsRepository.Insert(brand);
        var brandDto = _mapper.Map<BrandDto>(brand);

        return brandDto;
    }

    public async Task<BrandDto> UpdateBrand(BrandDto brandDto)
    {
        var brand = _mapper.Map<Brand>(brandDto);

        var result = await _brandsRepository.GetBrandById(brand.Id);

        if (result is null)
        {
            throw new NotFoundException(Messages.NotFound);
        }

        await _brandsRepository.Update(brand);

        return brandDto;
    }

    public async Task<BrandDto> DeleteBrandById(long id)
    {
        var isExist = await _brandsRepository.DoesBrandExist(id);

        if (!isExist)
        {
            throw new NotFoundException(Messages.NotFound);
        }

        var areAnyProducts = await _productsRepository.AnyProductOfBrandIdExists(id);

        if (areAnyProducts)
        {
            throw new DatabaseConflictException(Messages.HasProductsConflict);
        }

        var brand = await _brandsRepository.GetBrandById(id);
        var brandDto = _mapper.Map<BrandDto>(brand);
        await _brandsRepository.DeleteBrandById(id);

        return brandDto;
    }

    public async Task<IList<BrandDto>> GetAllBrands()
    {
        var brands = await _brandsRepository.GetAll();

        var brandsDto = _mapper.Map<IList<BrandDto>>(brands);

        return brandsDto;
    }

    public async Task<BrandDto> GetBrandById(long id)
    {
        var brand = await _brandsRepository.GetBrandById(id);

        if (brand is null)
        {
            throw new NotFoundException(Messages.NotFound);
        }

        var brandDto = _mapper.Map<BrandDto>(brand);

        return brandDto;
    }
}