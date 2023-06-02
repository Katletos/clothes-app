using Application.Dtos;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
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

    public async Task<BrandDto> AddBrand(BrandNameDto brandNameDto)
    {
        var brandExists = await _brandsRepository.DoesBrandExist(brandNameDto.Name);

        if (brandExists)
        {
            throw new BusinessRuleException(Messages.BrandNameUniqueConstraint);
        }

        var brand = _mapper.Map<Brand>(brandNameDto);
        await _brandsRepository.Insert(brand);
        var brandDto = _mapper.Map<BrandDto>(brand);

        return brandDto;
    }

    public async Task<BrandDto> UpdateBrand(long id, BrandNameDto brandNameDto)
    {
        var brand = _mapper.Map<BrandNameDto, Brand>(brandNameDto, opt => 
            opt.AfterMap((src, dest) => dest.Id = id));

        var exist = await _brandsRepository.DoesExist(id);

        if (!exist)
        {
            throw new NotFoundException(Messages.NotFound);
        }

        await _brandsRepository.Update(brand);

        var brandDto = _mapper.Map<BrandDto>(brand);
        
        return brandDto;
    }

    public async Task<BrandDto> DeleteBrandById(long id)
    {
        var exist = await _brandsRepository.DoesExist(id);

        if (!exist) 
        {
            throw new NotFoundException(Messages.NotFound);
        }

        var areAnyProducts = await _productsRepository.AnyProductOfBrandIdExists(id);

        if (areAnyProducts)
        {
            throw new BusinessRuleException(Messages.BrandDeleteConstraint);
        }

        var brand = await _brandsRepository.GetById(id);
        await _brandsRepository.DeleteBrand(brand);
        var brandDto = _mapper.Map<BrandDto>(brand);

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
        var brand = await _brandsRepository.GetById(id);

        if (brand is null)
        {
            throw new NotFoundException(Messages.NotFound);
        }

        var brandDto = _mapper.Map<BrandDto>(brand);

        return brandDto;
    }
}