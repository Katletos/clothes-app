using Application.Dtos.Brands;
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

    public async Task<BrandDto> Add(BrandInputDto brandInputDto)
    {
        var brandExists = await _brandsRepository.DoesExist(brandInputDto.Name);

        if (brandExists)
        {
            throw new BusinessRuleException(Messages.BrandNameUniqueConstraint);
        }

        var brand = _mapper.Map<Brand>(brandInputDto);
        await _brandsRepository.Insert(brand);
        var brandDto = _mapper.Map<BrandDto>(brand);

        return brandDto;
    }

    public async Task<BrandDto> Update(long id, BrandInputDto brandInputDto)
    {
        var brand = _mapper.Map<BrandInputDto, Brand>(brandInputDto, opt =>
            opt.AfterMap((_, dest) => dest.Id = id));

        var exist = await _brandsRepository.DoesExist(id);

        if (!exist)
        {
            throw new NotFoundException(Messages.BrandNotFound);
        }

        await _brandsRepository.Update(brand);

        var brandDto = _mapper.Map<BrandDto>(brand);

        return brandDto;
    }

    public async Task<BrandDto> DeleteById(long id)
    {
        var exist = await _brandsRepository.DoesExist(id);

        if (!exist)
        {
            throw new NotFoundException(Messages.BrandNotFound);
        }

        var areAnyProducts = await _productsRepository.AnyProductOfBrandIdExists(id);

        if (areAnyProducts)
        {
            throw new BusinessRuleException(Messages.BrandDeleteConstraint);
        }

        var brand = await _brandsRepository.GetById(id);
        await _brandsRepository.Delete(brand);
        var brandDto = _mapper.Map<BrandDto>(brand);

        return brandDto;
    }

    public async Task<IList<BrandDto>> GetAll()
    {
        var brands = await _brandsRepository.GetAll();

        var brandsDto = _mapper.Map<IList<BrandDto>>(brands);

        return brandsDto;
    }

    public async Task<BrandDto> GetById(long id)
    {
        var brand = await _brandsRepository.GetById(id);

        if (brand is null)
        {
            throw new NotFoundException(Messages.BrandNotFound);
        }

        var brandDto = _mapper.Map<BrandDto>(brand);

        return brandDto;
    }
}