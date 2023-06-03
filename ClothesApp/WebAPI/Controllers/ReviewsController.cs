using Application.Dtos.Reviews;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/Reviews")] 
public class ReviewsController : ControllerBase
{
    private readonly IReviewService _reviewService;   
    
    public ReviewsController(IReviewService reviewService)
    {
        _reviewService = reviewService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IList<ReviewDto>))]
    public async Task<ActionResult<IList<ReviewDto>>> GetAllReviews()
    {
        var reviewsDto = await _reviewService.GetAll();

        return Ok(reviewsDto);
    }
    
    [HttpGet("{id:long}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReviewDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<ReviewDto>> GetReviewById([FromRoute] long id)
    {
        var reviewDto = await _reviewService.GetById(id);
        
        return Ok(reviewDto);
    }

    [HttpDelete("{id:long}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReviewDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> DeleteReviewById([FromRoute] long id)
    {
        var reviewDto = await _reviewService.DeleteById(id);

        return Ok(reviewDto);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReviewDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<ActionResult> AddReview([FromBody] ReviewInputDto reviewInputDto)
    {
        var reviewDto = await _reviewService.Add(reviewInputDto);
        
        return Ok(reviewDto);
    }
    
    [HttpPut("{id:long}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReviewDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<ActionResult> UpdateReview([FromRoute] long id, [FromBody] ReviewInputDto reviewInputDto)
    {
        var reviewDto = await _reviewService.Update(id, reviewInputDto);
        
        return Ok(reviewDto);
    }
}