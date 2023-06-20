using Application;
using Application.Dtos.Orders;
using Application.Exceptions;
using AutoMapper;
using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
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

        await orderService.Add(testCase.OrderInputDto);

        var order = await Context.Orders.FirstAsync(o => o.UserId == testCase.User.Id 
                                                            && o.AddressId == testCase.Address.Id);
        var orderDto = Mapper.Map<OrderDto>(order);
        orderDto.Should().BeEquivalentTo(testCase.ExpectedResult);
    }
    
    [Theory]
    [ClassData(typeof(AddOrderTestData))]
    public async Task AddOrder_WhereCalledWithNonexistentUser_ThrowsNotFound(AddOrderTestCase testCase)
    {
        var orderService = CreateOrderService(Context);
        Context.Addresses.Add(testCase.Address);
        Context.Products.Add(testCase.Product);
        await Context.SaveChangesAsync();
        
        Func<Task> act = async () => await orderService.Add(testCase.OrderInputDto);
        
        var exception = await Assert.ThrowsAsync<NotFoundException>(act);
        exception.Message.Should().BeEquivalentTo(Messages.UserNotFound);
    }
    
    [Theory]
    [ClassData(typeof(AddOrderTestData))]
    public async Task AddOrder_WhereCalledWithUnrelatedUserAddress_ThrowsBusinessRule(AddOrderTestCase testCase)
    {
        var orderService = CreateOrderService(Context);
        var userId = testCase.User.Id;
        var address = testCase.Address;
        address.UserId = userId - 1; 
        Context.Users.Add(testCase.User);
        Context.Addresses.Add(address);
        Context.Products.Add(testCase.Product);
        await Context.SaveChangesAsync();
        
        Func<Task> act = async () => await orderService.Add(testCase.OrderInputDto);
        
        var exception = await Assert.ThrowsAsync<BusinessRuleException>(act);
        exception.Message.Should().BeEquivalentTo(Messages.AddressUserConstraint);
    }

    [Theory]
    [ClassData(typeof(ReserveOrderTestData))]
    public async Task ReserveOrderProduct_WhereReservedFiveProducts(ReserveOrderTestCase testCase)
    {
        var orderService = CreateOrderService(Context);
        Context.Products.Add(testCase.Product);
        await Context.SaveChangesAsync();
        
        await orderService.ReserveOrderProducts(testCase.OrderInputDto);

        var product = await Context.Products.FindAsync(testCase.Product.Id);
        product.Quantity.Should().Be(testCase.ExpectedResult.Quantity);
    }
    
    [Theory]
    [ClassData(typeof(ReserveOrderBusinessRuleExceptionTestData))]
    public async Task ReserveOrderProduct_WhereReservedQuantityGreaterThanInStock_ThrowsBusinessRule(ReserveOrderBusinessRuleExceptionTestCase testCase)
    {
        var orderService = CreateOrderService(Context);
        Context.Products.Add(testCase.Product);
        await Context.SaveChangesAsync();
        
        Func<Task> act = async () => await orderService.ReserveOrderProducts(testCase.OrderInputDto);

        var exception = await Assert.ThrowsAsync<BusinessRuleException>(act);
        exception.Message.Should().BeEquivalentTo(Messages.ProductOutOfStock);
    }  
    
    [Theory]
    [ClassData(typeof(ReserveOrderNotFoundExceptionTestData))]
    public async Task ReserveOrderProduct_WhereReserveNonexistentProduct_ThrowsNotFound(ReserveOrderNotFoundExceptionTestCase testCase)
    {
        var orderService = CreateOrderService(Context);
        Context.Products.Add(testCase.Product);
        await Context.SaveChangesAsync();
        
        Func<Task> act = async () => await orderService.ReserveOrderProducts(testCase.OrderInputDto);
        
        var exception = await Assert.ThrowsAsync<NotFoundException>(act);
        exception.Message.Should().BeEquivalentTo(Messages.ProductNotFound);
    }
}
