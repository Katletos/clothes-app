using Application.Dtos;
using Application.Repositories;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

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
        var brandExists = await _brandsRepository.IsExistAsync(createBrandDto.Name);

        if (brandExists)
        {
            throw new DuplicationException();
        }

        var brand = _mapper.Map<Brand>(createBrandDto);
        await _brandsRepository.InsertAsync(brand);
        var brandDto = _mapper.Map<BrandDto>(brand);

        return brandDto;
    }

    public async Task<BrandDto> UpdateBrandAsync(BrandDto brandDto)
    {
        var brand = _mapper.Map<Brand>(brandDto);

        brand.Name = brandDto.Name;

        var result = await _brandsRepository.GetByIdAsync(brand.Id);

        if (result is null)
        {
            throw new NotFoundException();
        }

        await _brandsRepository.UpdateAsync(brand);

        return brandDto;
    }

    public async Task<BrandDto> DeleteBrandById(long id)
    {
        var isExist = await _brandsRepository.IsExistAsync(id);

        if (!isExist)
        {
            throw new NotFoundException();
        }

        var isAny = await _productRepository.AnyProductOfBrandIdExists(id);

        if (isAny)
        {
            throw new RelationExistException();
        }

        var brand = await _brandsRepository.GetByIdAsync(id);
        var brandDto = _mapper.Map<BrandDto>(brand);
        await _brandsRepository.DeleteByIdAsync(id);

        return brandDto;
    }

    public async Task<IReadOnlyCollection<BrandDto>> GetAllBrandsAsync()
    {
        var brands = await _brandsRepository.GetAll().ToListAsync();

        var brandsDto = _mapper.Map<IReadOnlyCollection<BrandDto>>(brands);

        return brandsDto;
    }

    public async Task<BrandDto> GetBrandByIdAsync(long id)
    {
        var brand = await _brandsRepository.GetByIdAsync(id);

        var brandDto = _mapper.Map<BrandDto>(brand);

        return brandDto;
    }
}