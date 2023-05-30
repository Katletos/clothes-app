using Application.Dtos;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/Brands")] 
public class BrandsController : ControllerBase
{
    private readonly IBrandService _brandService;   
    
    public BrandsController(IBrandService brandService)
    {
       _brandService = brandService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<BrandDto>>> GetAllBrands()
    {
        var brandsDtos = await _brandService.GetAllBrandsAsync();

        return Ok(brandsDtos);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<BrandDto>> GetBrandById([FromRoute] long id)
    {
        var brandDto = await _brandService.GetBrandByIdAsync(id);
        
        return Ok(brandDto);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBrandById([FromRoute] long id)
    {
        var brandDto = await _brandService.DeleteBrandByIdAsync(id);

        return Ok(brandDto);
    }

    [HttpPost]
    public async Task<ActionResult> AddBrand([FromBody] CreateBrandDto createBrandDto)
    {
        var brandDto = await _brandService.AddBrandAsync(createBrandDto);
        
        return Ok(brandDto);
    }
    
    [HttpPut]
    public async Task<ActionResult> UpdateBrand([FromBody] BrandDto brandDto)
    {
        await _brandService.UpdateBrandAsync(brandDto);
        
        return Ok(brandDto);
    }
}