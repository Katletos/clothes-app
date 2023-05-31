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
        var brandsDtos = await _brandService.GetAllBrands();

        return Ok(brandsDtos);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<BrandDto>> GetBrandById([FromRoute] long id)
    {
        var brandDto = await _brandService.GetBrandById(id);
        
        return Ok(brandDto);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBrandById([FromRoute] long id)
    {
        var brandDto = await _brandService.DeleteBrandById(id);

        return Ok(brandDto);
    }

    [HttpPost]
    public async Task<ActionResult> AddBrand([FromBody] BrandNameDto brandNameDto)
    {
        var brandDto = await _brandService.AddBrand(brandNameDto);
        
        return Ok(brandDto);
    }
    
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateBrand([FromRoute] long id, [FromBody] BrandNameDto brandNameDto)
    {
        var brandDto = await _brandService.UpdateBrand(id, brandNameDto);
        
        return Ok(brandDto);
    }
}