using Application.Dtos;

namespace Application.Services;

public interface IBrandService
{
    Task<BrandDto> DeleteBrandByIdAsync(long id);

    Task<IList<BrandDto>> GetAllBrandsAsync();

    Task<BrandDto> GetBrandByIdAsync(long id);

    Task<BrandDto> AddBrandAsync(CreateBrandDto createBrandDto);

    Task<BrandDto> UpdateBrandAsync(BrandDto brandDto);
}