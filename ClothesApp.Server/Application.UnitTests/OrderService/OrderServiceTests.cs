using Application;
using Application.Dtos.Orders;
using Application.Exceptions;
using Application.Services;
using AutoMapper;
using Domain.Enums;
using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using UnitTests.OrderService.AddOrder;

namespace UnitTests.OrderService;

public class OrderServiceTests : BaseTest
{
    private Application.Services.OrderService CreateOrderService(ClothesAppContext context)
    {
        var mapper = new Mapper(new MapperConfiguration(cfg
            => cfg.AddProfile<AppMappingProfile>()));
        var productRepository = new ProductsRepository(context);

        return new Application.Services.OrderService(
            new OrderRepository(context),
            mapper,
            new OrderTransactionRepository(context),
            new UserRepository(context),
            new AddressRepository(context),
            new OrderItemsRepository(context),
            productRepository,
            new CartItemsService(
                new CartItemsRepository(context),
                mapper,
                new UserRepository(context),
                productRepository));
    }

    [Theory]
    [ClassData(typeof(AddOrderTestData))]
    public async Task AddOrder_WhereCalledWithCorrectData(AddOrderTestCase testCase)
    {
        var orderService = CreateOrderService(Context);
        Context.CartItems.AddRange(testCase.CartItems);
        Context.Users.Add(testCase.User);
        Context.Addresses.Add(testCase.Address);
        Context.Products.Add(testCase.Product);
        await Context.SaveChangesAsync();

        await orderService.Add(testCase.OrderInputDto);

        var order = await Context.Orders.FirstAsync(o => o.UserId == testCase.User.Id
                                                         && o.AddressId == testCase.Address.Id);
        var orderDto = Mapper.Map<OrderDto>(order);
        orderDto.Should().NotBeNull();
        orderDto.OrderStatus.Should().Be(OrderStatusType.InReview);
        orderDto.UserId.Should().Be(testCase.User.Id);
        orderDto.CreatedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(20));
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
    [ClassData(typeof(AddOrderProductNotFoundTestData))]
    public async Task AddOrder_WhichContainsNonexistentProduct_ThrowsNotFound(
        AddOrderProductNotFoundTestCase testCase)
    {
        var orderService = CreateOrderService(Context);
        Context.Addresses.Add(testCase.Address);
        Context.Users.Add(testCase.User);
        Context.Products.Add(testCase.Product);
        await Context.SaveChangesAsync();

        Func<Task> act = async () => await orderService.Add(testCase.OrderInputDto);

        var exception = await Assert.ThrowsAsync<NotFoundException>(act);
        exception.Message.Should().BeEquivalentTo(Messages.ProductNotFound);
    }
}