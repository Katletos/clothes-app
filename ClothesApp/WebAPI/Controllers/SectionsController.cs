using Application.Dtos.Sections;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/sections")]
public class SectionsController : ControllerBase
{
    private readonly ISectionService _sectionService;

    public SectionsController(ISectionService sectionService)
    {
        _sectionService = sectionService;
    }

    [AllowAnonymous]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IList<SectionDto>))]
    public async Task<ActionResult<IList<SectionDto>>> GetAllSections()
    {
        var sectionDto = await _sectionService.GetAll();

        return Ok(sectionDto);
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IList<SectionDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<SectionDto>> GetSectionById([FromRoute] long id)
    {
        var sectionDto = await _sectionService.GetById(id);

        return Ok(sectionDto);
    }

    [Authorize(Policy = "Admin")]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SectionDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> DeleteSectionById([FromRoute] long id)
    {
        var sectionDto = await _sectionService.DeleteById(id);

        return Ok(sectionDto);
    }

    [Authorize(Policy = "Admin")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SectionDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<ActionResult> AddSection([FromBody] SectionInputDto sectionInputDto)
    {
        var sectionDto = await _sectionService.Add(sectionInputDto);

        return Ok(sectionDto);
    }

    [Authorize(Policy = "Admin")]
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SectionDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<ActionResult> UpdateSection([FromRoute] long id, [FromBody] SectionInputDto sectionInputDto)
    {
        var sectionDto = await _sectionService.Update(id, sectionInputDto);

        return Ok(sectionDto);
    }
}