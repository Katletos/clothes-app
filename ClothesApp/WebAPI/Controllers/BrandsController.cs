using Application.Dtos;
using Application.Services;
using Infrastructure.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/BrandsController")] 
public class BrandsController : ControllerBase
{
    private readonly IBrandService _brandService;   
    
    private abstract class Messages
    {
        public const string NotFoundString = "Brand with Id not found"; 
    
        public const string HasProductsConflictString = "Brand with Id has one or more product";
    
        public const string AlreadyExistConflictString = "Brand already exists";
    }
    
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

        if (brandDto is null)
        {
            return NotFound(Messages.NotFoundString);
        }
        
        return Ok(brandDto);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBrandById([FromRoute] long id)
    {
        try
        {
            var brandDto = await _brandService.DeleteBrandById(id);

            return Ok(brandDto);
        }
        catch (NotFoundException)
        {
            return NotFound(Messages.NotFoundString);
        }
        catch (RelationExistException)
        {
            return Conflict(Messages.HasProductsConflictString);
        }
    }

    [HttpPost]
    public async Task<ActionResult> AddBrand([FromQuery] CreateBrandDto createBrandDto)
    {
        BrandDto brandDto;
        
        try
        {
            brandDto = await _brandService.AddBrandAsync(createBrandDto);
        }
        catch (DuplicationException)
        {
            return Conflict(Messages.AlreadyExistConflictString);
        }
        
        return Ok(brandDto);
    }
    
    [HttpPut]
    public async Task<ActionResult> UpdateBrand([FromBody] BrandDto brandDto)
    {
        try
        {
            await _brandService.UpdateBrandAsync(brandDto);
        }
        catch (NotFoundException)
        {
            return NotFound(Messages.NotFoundString);
        }
        
        return Ok(brandDto);
    }
}