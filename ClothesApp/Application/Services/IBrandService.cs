using Application.Dtos;

namespace Application.Services;

public interface IBrandService
{
    Task<BrandDto> DeleteBrandById(long id);

    Task<IReadOnlyCollection<BrandDto>> GetAllBrandsAsync();

    Task<BrandDto> GetBrandByIdAsync(long id);

    Task<BrandDto> AddBrandAsync(CreateBrandDto createBrandDto);

    Task<BrandDto> UpdateBrandAsync(BrandDto brandDto);
}