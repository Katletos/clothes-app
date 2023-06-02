using Application.Dtos;

namespace Application.Interfaces.Services;

public interface IBrandService
{
    Task<BrandDto> DeleteBrandById(long id);

    Task<IList<BrandDto>> GetAllBrands();

    Task<BrandDto> GetBrandById(long id);

    Task<BrandDto> AddBrand(BrandNameDto brandNameDto);

    Task<BrandDto> UpdateBrand(long id, BrandNameDto brandNameDto);
}