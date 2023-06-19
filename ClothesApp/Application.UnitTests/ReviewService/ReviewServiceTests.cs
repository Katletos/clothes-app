using Application;
using Application.Exceptions;
using AutoMapper;
using Infrastructure;
using Infrastructure.Repositories;
using UnitTests.ReviewService.GetById;
using UnitTests.ReviewService.GetByProductId;

namespace UnitTests.ReviewService;

public class ReviewServiceTests : BaseTest
{
    private Application.Services.ReviewService GetReviewService(ClothesAppContext context)
    {
        return new Application.Services.ReviewService(
            new ReviewsRepository(context),
            new Mapper(new MapperConfiguration(cfg
                => cfg.AddProfile<AppMappingProfile>())),
            new ProductsRepository(context),
            new UserRepository(context)
            );
    }

    [Theory]
    [ClassData(typeof(GetByProductIdTestData))]
    public async Task GetByProductId_WhereCalledWithCorrectData(GetByProductIdTestCase testCase)
    {
        var reviewService = GetReviewService(Context);
        Context.Reviews.AddRange(testCase.ReviewFromRepository);
        await Context.SaveChangesAsync();
        
        var result = await reviewService.GetByProductId(testCase.ProductId);
       
        result.Should().BeEquivalentTo(testCase.ExpectedResult);
    }

    [Theory]
    [ClassData(typeof(GetByIdTestData))]
    public async Task GetById_WhereCalledWithUndiscoveredReviewId_ThrowsNotFound(GetByIdTestCase testCase)
    {
        var reviewService = GetReviewService(Context);
        Context.Reviews.AddRange(testCase.ReviewFromRepository);
        await Context.SaveChangesAsync();
        
        Func<Task> act = async () => await reviewService.GetById(testCase.Id - 1);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Theory]
    [ClassData(typeof(GetByIdTestData))]
    public async Task GetById_WhereCalledWithValidData(GetByIdTestCase testCase)
    {
        var reviewService = GetReviewService(Context);
        Context.Reviews.AddRange(testCase.ReviewFromRepository);
        await Context.SaveChangesAsync();

        var result = await reviewService.GetById(testCase.Id);

        result.Should().BeEquivalentTo(testCase.ExpectedResult);
    }
}
