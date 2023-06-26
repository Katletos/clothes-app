using Application.Dtos.Brands;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Authentication;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/brands")]
public class BrandsController : ControllerBase
{
    private readonly IBrandService _brandService;

    public BrandsController(IBrandService brandService)
    {
        _brandService = brandService;
    }

    [AllowAnonymous]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IList<BrandDto>))]
    public async Task<ActionResult<IList<BrandDto>>> GetAllBrands()
    {
        var brandDtos = await _brandService.GetAll();

        return Ok(brandDtos);
    }

    [Authorize(Policy = Policies.Admin)]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BrandDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> DeleteBrandById([FromRoute] long id)
    {
        var brandDto = await _brandService.DeleteById(id);

        return Ok(brandDto);
    }

    [Authorize(Policy = Policies.Admin)]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BrandDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<ActionResult> AddBrand([FromBody] BrandInputDto brandInputDto)
    {
        var brandDto = await _brandService.Add(brandInputDto);

        return Ok(brandDto);
    }

    [Authorize(Policy = Policies.Admin)]
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BrandDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<ActionResult> UpdateBrand([FromRoute] long id, [FromBody] BrandInputDto brandInputDto)
    {
        var brandDto = await _brandService.Update(id, brandInputDto);

        return Ok(brandDto);
    }
}