using Application.Dtos.Brands;

namespace Application.Interfaces.Services;

public interface IBrandService
{
    Task<BrandDto> DeleteById(long id);

    Task<IList<BrandDto>> GetAll();

    Task<BrandDto> GetById(long id);

    Task<BrandDto> Add(BrandInputDto brandInputDto);

    Task<BrandDto> Update(long id, BrandInputDto brandInputDto);
}