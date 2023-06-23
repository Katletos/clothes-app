using Application;
using Application.Dtos.Reviews;
using Application.Exceptions;
using Application.Interfaces.Services;
using Domain.Enums;
using Infrastructure.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Authentication;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/reviews")]
public class ReviewsController : ControllerBase
{
    private readonly IReviewService _reviewService;

    private readonly IUserService _userService;

    public ReviewsController(IReviewService reviewService, IUserService userService)
    {
        _reviewService = reviewService;
        _userService = userService;
    }

    [Authorize(Policy = Policies.Customer)]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReviewDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<ActionResult> AddProductReview([FromBody] ReviewInputDto reviewInputDto)
    {
        var userClaimId = User.FindFirst(CustomClaims.Id);
        var userId = Convert.ToInt64(userClaimId?.Value);

        var reviewDto = await _reviewService.Add(reviewInputDto, userId);

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
        var userClaimId = User.FindFirst(CustomClaims.Id);
        var userId = Convert.ToInt64(userClaimId?.Value);

        var user = await _userService.GetById(userId);
        var review = await _reviewService.GetById(id);

        if (user.UserType == UserType.Admin || review.UserId == userId)
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
        var userClaimId = User.FindFirst(CustomClaims.Id);
        var userId = Convert.ToInt64(userClaimId?.Value);

        var user = await _userService.GetById(userId);
        var review = await _reviewService.GetById(id);

        if (user.UserType == UserType.Admin || review.UserId == userId)
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