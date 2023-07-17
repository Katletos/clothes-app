using Application.Dtos.Products;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Authentication;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<ActionResult> GetProductById([FromRoute] long id)
    {
        var productDto = await _productService.GetById(id);

        return Ok(productDto);
    }

    [AllowAnonymous]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IList<ProductDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<ActionResult> GetProductsBySectionAndCategory([FromQuery] long sectionId,
        [FromQuery] long categoryId)
    {
        var productDtos = await _productService.GetProductsBySectionAndCategory(sectionId, categoryId);

        return Ok(productDtos);
    }

    [Authorize(Policy = Policies.Admin)]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<ActionResult> AddProduct([FromBody] ProductInputDto productInputDto)
    {
        var products = await _productService.Add(productInputDto);

        return Ok(products);
    }

    [Authorize(Policy = Policies.Admin)]
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<ActionResult> UpdateProduct([FromRoute] long id, [FromBody] ProductInputDto productInputDto)
    {
        var productDtos = await _productService.Update(id, productInputDto);

        return Ok(productDtos);
    }

    [Authorize(Policy = Policies.Admin)]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<ActionResult> DeleteProductById([FromRoute] long id)
    {
        var productDto = await _productService.DeleteById(id);

        return Ok(productDto);
    }

    [AllowAnonymous]
    [HttpGet("brands/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IList<ProductDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<ActionResult> GetProductsByBrandId([FromRoute] long id)
    {
        var products = await _productService.GetProductsByBrandId(id);

        return Ok(products);
    }
}