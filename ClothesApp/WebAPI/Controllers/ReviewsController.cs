using Application;
using Application.Dtos.Reviews;
using Application.Exceptions;
using Application.Interfaces.Services;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Authentication;

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

    [Authorize(Policy = Policies.Customer)]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReviewDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<ActionResult> AddProductReview([FromBody] ReviewInputDto reviewInputDto)
    {
        var reviewDto = await _reviewService.Add(reviewInputDto, User.GetUserId());

        return Ok(reviewDto);
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReviewDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<ReviewDto>> GetReviewById([FromRoute] long id)
    {
        var reviewDto = await _reviewService.GetById(id);

        return Ok(reviewDto);
    }

    [AllowAnonymous]
    [HttpGet("users/{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IList<ReviewDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<ActionResult> GetReviewByUserId([FromRoute] long userId)
    {
        var reviewsDto = await _reviewService.GetByUserId(userId);

        return Ok(reviewsDto);
    }

    [AllowAnonymous]
    [HttpGet("products/{productId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IList<ReviewDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<ActionResult> GetProductReviews([FromRoute] long productId)
    {
        var productReviews = await _reviewService.GetByProductId(productId);

        return Ok(productReviews);
    }

    [Authorize]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReviewDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> DeleteReviewById([FromRoute] long id)
    {
        var review = await _reviewService.GetById(id);

        if (User.GetUserType() == UserType.Admin || review.UserId == User.GetUserId())
        {
            var reviewDto = await _reviewService.DeleteById(id);

            return Ok(reviewDto);
        }
        else
        {
            throw new BusinessRuleException(Messages.AuthorizationConstraint);
        }
    }

    [Authorize]
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReviewDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<ActionResult> UpdateReview([FromRoute] long id, [FromBody] UpdateReviewDto updateReviewDto)
    {
        var review = await _reviewService.GetById(id);

        if (User.GetUserType() == UserType.Admin || review.UserId == User.GetUserId())
        {
            var reviewDto = await _reviewService.Update(id, updateReviewDto);
            return Ok(reviewDto);
        }
        else
        {
            throw new BusinessRuleException(Messages.AuthorizationConstraint);
        }
    }
}