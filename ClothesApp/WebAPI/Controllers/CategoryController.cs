using Application;
using Application.Dtos.Category;
using Application.Dtos.SectionCategories;
using Application.Interfaces.Services;
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

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CategoryTree))]
    public async Task<ActionResult> GetCategoryTree()
    {
        var tree = await _categoryService.BuildCategoryTree();

        return Ok(tree);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CategoryDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<ActionResult> AddCategory([FromBody] CategoryInputDto categoryInputDto)
    {
        var categoryDto = await _categoryService.Add(categoryInputDto);
        
        return Ok(categoryDto);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CategoryDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<ActionResult> UpdateCategory([FromRoute] long id, [FromBody] CategoryInputDto categoryInputDto)
    {
        var categoryDto = await _categoryService.Update(id, categoryInputDto);
        
        return Ok(categoryDto);
    }
    
    [HttpPut("{id}/link-to-section/{sectionId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SectionCategoryDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<ActionResult> LinkCategoryToSection([FromRoute] long id, [FromRoute] long sectionId)
    {
        var sectionCategoryDto = await _categoryService.LinkCategoryToSection(id, sectionId);
        
        return Ok(sectionCategoryDto);
    }
}