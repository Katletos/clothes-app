using Application.Dtos.Reviews;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/reviews")] 
public class ReviewsController : ControllerBase
{
    private readonly IReviewService _reviewService;   
    
    public ReviewsController(IReviewService reviewService)
    {
        _reviewService = reviewService;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReviewDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<ActionResult> AddProductReview([FromBody] ReviewInputDto reviewInputDto)
    {
        var reviewDto = await _reviewService.Add(reviewInputDto);

        return Ok(reviewDto);
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReviewDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<ReviewDto>> GetReviewById([FromRoute] long id)
    {
        var reviewDto = await _reviewService.GetById(id);
        
        return Ok(reviewDto);
    }

    [HttpGet("users/{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IList<ReviewDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<ActionResult> GetReviewByUserId([FromRoute] long userId)
    {
        var reviewsDto = await _reviewService.GetByUserId(userId);

        return Ok(reviewsDto);
    }
    
    [HttpGet("products/{productId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IList<ReviewDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<ActionResult> GetProductReviews([FromRoute] long productId)
    {
        var productReviews = await _reviewService.GetByProductId(productId);

        return Ok(productReviews);
    }
    
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReviewDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> DeleteReviewById([FromRoute] long id)
    {
        var reviewDto = await _reviewService.DeleteById(id);

        return Ok(reviewDto);
    }
    
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReviewDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<ActionResult> UpdateReview([FromRoute] long id, [FromBody] UpdateReviewDto updateReviewDto)
    {
        var reviewDto = await _reviewService.Update(id, updateReviewDto);
        
        return Ok(reviewDto);
    }
}