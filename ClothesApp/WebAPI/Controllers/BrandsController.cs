using System.Runtime.CompilerServices;
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
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IList<BrandDto>))]
    public async Task<ActionResult<IList<BrandDto>>> GetAllBrands()
    {
        var brandsDtos = await _brandService.GetAllBrands();

        return Ok(brandsDtos);
    }
    
    [HttpGet("{id:long}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BrandDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<BrandDto>> GetBrandById([FromRoute] long id)
    {
        var brandDto = await _brandService.GetBrandById(id);
        
        return Ok(brandDto);
    }

    [HttpDelete("{id:long}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BrandDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> DeleteBrandById([FromRoute] long id)
    {
        var brandDto = await _brandService.DeleteBrandById(id);

        return Ok(brandDto);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BrandDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<ActionResult> AddBrand([FromBody] BrandNameDto brandNameDto)
    {
        var brandDto = await _brandService.AddBrand(brandNameDto);
        
        return Ok(brandDto);
    }
    
    [HttpPut("{id:long}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BrandDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<ActionResult> UpdateBrand([FromRoute] long id, [FromBody] BrandNameDto brandNameDto)
    {
        var brandDto = await _brandService.UpdateBrand(id, brandNameDto);
        
        return Ok(brandDto);
    }
}