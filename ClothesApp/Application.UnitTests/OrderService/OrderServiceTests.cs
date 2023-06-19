using Application;
using AutoMapper;
using Infrastructure;
using Infrastructure.Repositories;
using UnitTests.OrderService.AddOrder;
using UnitTests.OrderService.ReserveOrderProduct;

namespace UnitTests.OrderService;

public class OrderServiceTests : BaseTest
{
    private Application.Services.OrderService CreateOrderService(ClothesAppContext context)
    {
        return new Application.Services.OrderService(
            new OrderRepository(context),
            new Mapper(new MapperConfiguration(cfg 
                => cfg.AddProfile<AppMappingProfile>())),
            new OrderTransactionRepository(context),
            new UserRepository(context),
            new AddressRepository(context),
            new OrderItemsRepository(context),
            new ProductsRepository(context));
    }
    
    [Theory]
    [ClassData(typeof(AddOrderTestData))]
    public async Task AddOrder_WhereCalledWithCorrectData(AddOrderTestCase testCase)
    {
        var orderService = CreateOrderService(Context);
        Context.Users.Add(testCase.User);
        Context.Addresses.Add(testCase.Address);
        Context.Products.Add(testCase.Product);
        await Context.SaveChangesAsync();

        var act = await orderService.Add(testCase.OrderInputDto);

        act.Should().BeEquivalentTo(testCase.OrderDto);
    }

    [Theory]
    [ClassData(typeof(ReserveOrderTestData))]
    public async Task ReserveOrderProduct_WhereReservedFiveProducts(ReserveOrderTestCase testCase)
    {
        var orderService = CreateOrderService(Context);
        Context.Products.Add(testCase.Product);
        await Context.SaveChangesAsync();
        
        await orderService.ReserveOrderProducts(testCase.OrderInputDto);

        testCase.Product.Quantity.Should().Be(testCase.ExpectedResult.Quantity);
    }
}
