using Application.Dtos.Products;
using Application.Dtos.Reviews;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/products")] 
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    private readonly IReviewService _reviewService;

    public ProductsController(IProductService productService, IReviewService reviewService)
    {
        _productService = productService;
        _reviewService = reviewService;
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IList<ProductDto>))]
    public async Task<ActionResult> GetProducts()
    {
        var products = await _productService.GetAll();

        return Ok(products);
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductDto))]
    public async Task<ActionResult> AddProduct([FromBody] ProductInputDto productInputDto)
    {
        var products = await _productService.Add(productInputDto);

        return Ok(products);
    }
    
    [HttpGet("brands/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IList<ProductDto>))]
    public async Task<ActionResult> GetProductsByBrandId([FromRoute] long id)
    {
        var products = await _productService.GetProductsByBrandId(id);

        return Ok(products);
    }
    
    [HttpGet("{id}/reviews")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IList<ReviewDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<ActionResult> GetProductReviews([FromRoute] long id)
    {
        var productReviews = await _reviewService.GetByProductId(id);

        return Ok(productReviews);
    }
    
    [HttpPost("{id}/reviews")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReviewDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<ActionResult> AddProductReview([FromRoute] long id, [FromBody] ReviewInputDto reviewInputDto)
    {
        var reviewDto = await _reviewService.Add(id, reviewInputDto);
        
        return Ok(reviewDto);
    }
}