using Application;
using Application.Dtos.Addresses;
using Application.Exceptions;
using AutoMapper;
using Bogus;
using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using UnitTests.AddressService.AddAddress;
using UnitTests.AddressService.GetAddress;
using UnitTests.AddressService.UpdateAddress;
using static UnitTests.EntitiesGenerator;

namespace UnitTests.AddressService;

public class AddressServiceTests : BaseTest
{
    private Application.Services.AddressService CreateAddressService(ClothesAppContext context)
    {
        return new Application.Services.AddressService(
            new AddressRepository(context),
            new UserRepository(context),
            new Mapper(new MapperConfiguration(cfg
                => cfg.AddProfile<AppMappingProfile>())));
    }

    [Theory]
    [ClassData(typeof(GetAddressesTestData))]
    public async Task GetAddresses_WhereCalledWithCorrectData(GetAddressTestCase testCase)
    {
        var addressService = CreateAddressService(Context);
        Context.Users.Add(testCase.User);
        Context.Addresses.AddRange(testCase.Addresses);
        await Context.SaveChangesAsync();

        var result = await addressService.GetAddresses(testCase.User.Id);

        result.Should().BeEquivalentTo(testCase.ExpectedResult);
    }

    [Theory]
    [ClassData(typeof(GetAddressesTestData))]
    public async Task GetAddresses_WhereCalledWithUndiscoveredUserId_ThrowNotFound(GetAddressTestCase testCase)
    {
        var addressService = CreateAddressService(Context);
        Context.Users.Add(testCase.User);
        await Context.SaveChangesAsync();

        Func<Task> act = async () => await addressService.GetAddresses(testCase.User.Id - 1);

        var exception = await Assert.ThrowsAsync<NotFoundException>(act);
        exception.Message.Should().BeEquivalentTo(Messages.UserNotFound);
    }

    [Theory]
    [ClassData(typeof(AddAddressTestData))]
    public async Task AddAddress_WhereCalledWithCorrectAddAddressDto(AddAddressTestCase testCase)
    {
        var addressService = CreateAddressService(Context);
        Context.Users.Add(testCase.User);
        await Context.SaveChangesAsync();

        await addressService.AddAddress(testCase.AddAddressDto);

        var address = await Context.Addresses.FirstAsync(a =>
            a.UserId == testCase.AddAddressDto.UserId && a.AddressLine == testCase.AddAddressDto.AddressLine);
        var addressDto = Mapper.Map<AddAddressDto>(address);
        addressDto.Should().BeEquivalentTo(testCase.AddAddressDto);
    }

    [Theory]
    [ClassData(typeof(UpdateAddressesTestData))]
    public async Task UpdateAddress_WhereCalledWithANonRelativeUserAddress_ThrowsBusinessRule(
        UpdateAddressTestCase testCase)
    {
        var addressService = CreateAddressService(Context);
        Context.Users.Add(testCase.User);
        var address = testCase.Address;
        address.UserId = -1;
        Context.Addresses.Add(testCase.Address);
        await Context.SaveChangesAsync();

        Func<Task> act = async () => await addressService.UpdateAddress(testCase.User.Id, testCase.AddressInputDto);

        var exception = await Assert.ThrowsAsync<BusinessRuleException>(act);
        exception.Message.Should().BeEquivalentTo(Messages.AddressUserConstraint);
    }

    [Fact]
    public async Task Update_WhereCalledWithUndiscoveredUserId_ThrowsNotFound()
    {
        var addressService = CreateAddressService(Context);
        var faker = new Faker();
        var user = GenerateUser();
        var addressInputDto = new AddressInputDto()
        {
            Id = faker.Random.Long(1),
            AddressLine = faker.Address.FullAddress(),
        };
        Context.Users.Add(user);

        Func<Task> act = async () => await addressService.UpdateAddress(user.Id - 1, addressInputDto);

        var exception = await Assert.ThrowsAsync<NotFoundException>(act);
        exception.Message.Should().BeEquivalentTo(Messages.UserNotFound);
    }

    [Fact]
    public async Task UpdateAddress_WhereCalledWithUndiscoveredAddressId_ThrowsNotFound()
    {
        var addressService = CreateAddressService(Context);
        var faker = new Faker();
        var addressInputDto = new AddressInputDto()
        {
            Id = faker.Random.Long(1),
            AddressLine = faker.Address.FullAddress(),
        };
        var user = GenerateUser();
        Context.Users.Add(user);
        await Context.SaveChangesAsync();

        Func<Task> act = async () => await addressService.UpdateAddress(user.Id, addressInputDto);

        var exception = await Assert.ThrowsAsync<NotFoundException>(act);
        exception.Message.Should().BeEquivalentTo(Messages.AddressNotFound);
    }

    [Theory]
    [ClassData(typeof(UpdateAddressesTestData))]
    public async Task UpdateAddress_WhereCalledWithCorrectData(UpdateAddressTestCase testCase)
    {
        var addressService = CreateAddressService(Context);
        Context.Users.Add(testCase.User);
        Context.Addresses.Add(testCase.Address);
        await Context.SaveChangesAsync();

        await addressService.UpdateAddress(testCase.User.Id, testCase.AddressInputDto);

        var address = await Context.Addresses.FirstAsync(a => a.UserId == testCase.User.Id
                                                              && a.AddressLine == testCase.ExpectedResult.AddressLine);
        var addressDto = Mapper.Map<AddressDto>(address);
        addressDto.Should().BeEquivalentTo(testCase.ExpectedResult);
    }
}