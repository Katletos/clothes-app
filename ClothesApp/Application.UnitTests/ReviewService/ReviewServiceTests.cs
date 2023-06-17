using Application.Dtos.Reviews;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using AutoMapper;
using UnitTests.ReviewService.GetById;
using UnitTests.ReviewService.GetByProductId;

namespace UnitTests.ReviewService;

public class ReviewServiceTests
{
    private readonly Mock<IReviewsRepository> _reviewsRepositoryMock;

    private readonly Mock<IProductsRepository> _productsRepositoryMock;

    private readonly Mock<IUserRepository> _userRepositoryMock;
    
    private readonly Mock<IMapper> _mapperMock;

    public ReviewServiceTests()
    {
        _reviewsRepositoryMock = new Mock<IReviewsRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _productsRepositoryMock = new Mock<IProductsRepository>();
        _mapperMock = new Mock<IMapper>();
    }

    private Application.Services.ReviewService GetReviewService()
    {
        return new Application.Services.ReviewService(_reviewsRepositoryMock.Object,
            _mapperMock.Object, _productsRepositoryMock.Object, _userRepositoryMock.Object);
    }

    [Theory]
    [ClassData(typeof(GetByProductIdTestData))]
    public async Task GetByProductId_WhereCalledWithCorrectData(GetByProductIdTestCase testCase)
    {
        var reviewService = GetReviewService();
        var productId = testCase.ProductId;
        _reviewsRepositoryMock.Setup(x => x.FindByCondition(r => r.ProductId == productId))
            .ReturnsAsync(testCase.ReviewFromRepository);
        _mapperMock.Setup(x => x.Map<IList<ReviewDto>>(testCase.ReviewFromRepository))
            .Returns(testCase.ExpectedResult);

        var result = await reviewService.GetByProductId(testCase.ProductId);
       
        result.Should().BeEquivalentTo(testCase.ExpectedResult);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(25)]
    [InlineData(123123)]
    public async Task GetById_WhereCalledWithUndiscoveredReviewId_ThrowsNotFound(long reviewId)
    {
        var reviewService = GetReviewService();
        _reviewsRepositoryMock.Setup(x => x.DoesExist(It.Is<long>(l => l == reviewId))).ReturnsAsync(false);
        
        Func<Task> act = async () => await reviewService.GetById(1);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Theory]
    [ClassData(typeof(GetByIdTestData))]
    public async Task GetById_WhereCalledWithValidData(GetByIdTestCase testCase)
    {
        var reviewService = GetReviewService();
        _reviewsRepositoryMock.Setup(x => x.DoesExist(
            It.Is<long>(l => l == testCase.Id))).ReturnsAsync(true);
        _reviewsRepositoryMock.Setup(x => x.GetById(
                It.Is<long>(l => l == testCase.Id)))
            .ReturnsAsync(testCase.ReviewFromRepository);
        _mapperMock.Setup(x => x.Map<ReviewDto>(testCase.ReviewFromRepository))
            .Returns(testCase.ExpectedResult);

        var result = await reviewService.GetById(testCase.Id);

        result.Should().BeEquivalentTo(testCase.ExpectedResult);
    }
}
