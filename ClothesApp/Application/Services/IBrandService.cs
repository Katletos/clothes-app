using Application.Dtos;

namespace Application.Services;

public interface IBrandService
{
    Task<BrandDto> DeleteBrandById(long id);

    Task<IList<BrandDto>> GetAllBrands();

    Task<BrandDto> GetBrandById(long id);

    Task<BrandDto> AddBrand(CreateBrandDto createBrandDto);

    Task<BrandDto> UpdateBrand(BrandDto brandDto);
}