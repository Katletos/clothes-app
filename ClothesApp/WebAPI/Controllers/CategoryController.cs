using Application.Dtos.Categories;
using Application.Dtos.SectionCategories;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/categories")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [AllowAnonymous]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IList<CategoryDto>))]
    public async Task<ActionResult> GetTopLevelCategories()
    {
        var categories = await _categoryService.GetTopLevelCategories();

        return Ok(categories);
    }

    [AllowAnonymous]
    [HttpGet("tree")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CategoryTree))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<ActionResult> GetTreeByParentCategoryId([FromQuery] long id)
    {
        var tree = await _categoryService.BuildCategoryTree(id);

        return Ok(tree);
    }

    [Authorize(Policy = "Admin")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CategoryDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<ActionResult> AddCategory([FromBody] CategoryInputDto categoryInputDto)
    {
        var categoryDto = await _categoryService.Add(categoryInputDto);

        return Ok(categoryDto);
    }

    [Authorize(Policy = "Admin")]
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CategoryDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<ActionResult> UpdateCategory([FromRoute] long id, [FromBody] CategoryInputDto categoryInputDto)
    {
        var categoryDto = await _categoryService.Update(id, categoryInputDto);

        return Ok(categoryDto);
    }

    [Authorize(Policy = "Admin")]
    [HttpPut("{id}/link-to-section/{sectionId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SectionCategoryDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<ActionResult> LinkCategoryToSection([FromRoute] long id, [FromRoute] long sectionId)
    {
        var sectionCategoryDto = await _categoryService.LinkCategoryToSection(id, sectionId);

        return Ok(sectionCategoryDto);
    }
}