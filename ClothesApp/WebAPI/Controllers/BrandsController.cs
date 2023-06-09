using Application.Dtos.Brands;
using Application.Dtos.Products;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/brands")] 
public class BrandsController : ControllerBase
{
    private readonly IBrandService _brandService;  
    
    private readonly IProductService _productService;  
    
    public BrandsController(IBrandService brandService, IProductService productService)
    {
        _brandService = brandService;
        _productService = productService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IList<BrandDto>))]
    public async Task<ActionResult<IList<BrandDto>>> GetAllBrands()
    {
        var brandDtos = await _brandService.GetAll();

        return Ok(brandDtos);
    }
    
    [HttpGet("{id}/products")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IList<ProductDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<IList<ProductDto>>> GetBrandProducts([FromRoute] long id)
    {
        var productDtos = await _productService.GetProductsByBrandId(id);

        return Ok(productDtos);
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BrandDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<BrandDto>> GetBrandById([FromRoute] long id)
    {
        var brandDto = await _brandService.GetById(id);
        
        return Ok(brandDto);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BrandDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> DeleteBrandById([FromRoute] long id)
    {
        var brandDto = await _brandService.DeleteById(id);

        return Ok(brandDto);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BrandDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<ActionResult> AddBrand([FromBody] BrandInputDto brandInputDto)
    {
        var brandDto = await _brandService.Add(brandInputDto);
        
        return Ok(brandDto);
    }
    
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BrandDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<ActionResult> UpdateBrand([FromRoute] long id, [FromBody] BrandInputDto brandInputDto)
    {
        var brandDto = await _brandService.Update(id, brandInputDto);
        
        return Ok(brandDto);
    }
    
    [HttpPost("{brandId}/assign-product/{productId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<ActionResult> AssignProductToBrand([FromRoute] long brandId,[FromRoute] long productId)
    {
        var productDto = await _productService.AssignToBrand(productId, brandId);
        
        return Ok(productDto);
    }
    
    [HttpPost("unassign-product/{productId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<ActionResult> UnassignProductFromBrand([FromRoute] long productId)
    {
        var productDto = await _productService.UnassignFromBrand(productId);
        
        return Ok(productDto);
    }
}