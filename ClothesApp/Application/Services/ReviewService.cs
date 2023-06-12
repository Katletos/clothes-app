using Application.Dtos.Reviews;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Entities;

namespace Application.Services;

public class ReviewService : IReviewService
{
    private readonly IReviewsRepository _reviewsRepository;

    private readonly IProductsRepository _productsRepository;

    private readonly IUserRepository _userRepository;
    
    private readonly IMapper _mapper;

    public ReviewService(IReviewsRepository reviewsRepository, IMapper mapper, IProductsRepository productsRepository, IUserRepository userRepository)
    {
        _reviewsRepository = reviewsRepository;
        _mapper = mapper;
        _productsRepository = productsRepository;
        _userRepository = userRepository;
    }

    public async Task<ReviewDto> Add(ReviewInputDto reviewInputDto)
    {
        var exist = await _productsRepository.DoesExist(reviewInputDto.ProductId);

        if (!exist)
        {
            throw new NotFoundException(Messages.ProductNotFound);
        }

        exist = await _userRepository.DoesExist(reviewInputDto.UserId);

        if (!exist)
        {
            throw new NotFoundException(Messages.UserNotFound);
        }
        
        exist = await _reviewsRepository.DoesReviewExist(reviewInputDto);

        if (exist)
        {
            throw new BusinessRuleException(Messages.ReviewUniqueConstraint);
        }
        
        var review = _mapper.Map<Review>(reviewInputDto);
        await _reviewsRepository.Insert(review);
        var reviewDto = _mapper.Map<ReviewDto>(review);
        
        return reviewDto;
    }

    public async Task<ReviewDto> Update(long id, UpdateReviewDto updateReviewDto)
    {
        var exist = await _reviewsRepository.DoesExist(id);

        if (!exist)
        {
            throw new NotFoundException(Messages.ReviewNotFound);
        }

        var review = await _reviewsRepository.GetById(id);
        var reviewDto = _mapper.Map<Review, ReviewDto>(review, opt =>
            opt.BeforeMap((src, _) =>
            {
                src.Comment = updateReviewDto.Comment;
                src.Rating = updateReviewDto.Rating;
                src.Title = updateReviewDto.Title;
            }));
        await _reviewsRepository.Update(review);

        return reviewDto;
    }

    public async Task<ReviewDto> DeleteById(long id)
    {
        var exist = await _reviewsRepository.DoesExist(id);

        if (!exist) 
        {
            throw new NotFoundException(Messages.ReviewNotFound);
        }
        
        var review = await _reviewsRepository.GetById(id);
        await _reviewsRepository.Delete(review);
        var reviewDto = _mapper.Map<ReviewDto>(review);

        return reviewDto;
    }

    public async Task<IList<ReviewDto>> GetByProductId(long id)
    {
        var reviews = await _reviewsRepository.FindByCondition(r => r.ProductId == id);

        var reviewsDto = _mapper.Map<IList<ReviewDto>>(reviews);

        return reviewsDto;
    }

    public async Task<IList<ReviewDto>> GetByUserId(long userId)
    {
        var reviews = await _reviewsRepository.FindByCondition(r => r.UserId == userId);

        var reviewsDto = _mapper.Map<IList<ReviewDto>>(reviews);

        return reviewsDto;
    }

    public async Task<ReviewDto> GetById(long id)
    {
        var review = await _reviewsRepository.GetById(id);

        if (review is null)
        {
            throw new NotFoundException(Messages.ReviewNotFound);
        }

        var reviewDto = _mapper.Map<ReviewDto>(review);

        return reviewDto;
    }

    public async Task<IList<ReviewDto>> GetAll()
    {
        var reviews = await _reviewsRepository.GetAll();

        var reviewsDto = _mapper.Map<IList<ReviewDto>>(reviews);

        return reviewsDto;
    }
}