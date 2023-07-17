using Application.Dtos.Products;

namespace Application.Interfaces.Services;

public interface IProductService
{
    Task<ProductDto> GetById(long id);

    Task<ProductDto> Add(ProductInputDto productInputDto);

    Task<ProductDto> Update(long id, ProductInputDto productInputDto);

    Task<ProductDto> DeleteById(long id);

    Task<IList<ProductDto>> GetProductsBySectionAndCategory(long sectionId, long categoryId);

    Task<IList<ProductDto>> GetProductsByBrandId(long brandId);
}