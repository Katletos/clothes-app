using Application;
using Application.Dtos.Addresses;
using Application.Exceptions;
using AutoMapper;
using Bogus;
using Domain.Entities;
using Domain.Enums;
using Infrastructure;
using Infrastructure.Repositories;
using UnitTests.AddressService.AddAddress;
using UnitTests.AddressService.GetAddress;
using UnitTests.AddressService.UpdateAddress;

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

        await act.Should().ThrowAsync<NotFoundException>();
    }

    private User GenerateUser()
    {
        var faker = new Faker();
        return new User()
        {
            Id = faker.Random.Long(1),
            Email = faker.Internet.Email(),
            Password = faker.Internet.Password(),
            Phone = faker.Phone.PhoneNumber(),
            CreatedAt = DateTime.Now,
            UserType = faker.PickRandom<UserType>(),
            FirstName = faker.Name.FirstName(),
            LastName = faker.Name.LastName(),
        };
    }

    [Theory]
    [ClassData(typeof(AddAddressTestData))]
    public async Task AddAddress_WhereCalledWithCorrectAddAddressDto(AddAddressTestCase testCase)
    {
        var addressService = CreateAddressService(Context);
        Context.Users.Add(testCase.User);
        await Context.SaveChangesAsync();
        
        var act = await addressService.AddAddress(testCase.AddAddressDto);
        
        act.Should().BeEquivalentTo(testCase.AddAddressDto);
    }
    
    [Theory]
    [ClassData(typeof(UpdateAddressesTestData))]
    public async Task UpdateAddress_WhereCalledWithANonRelativeUserAddress_ThrowsBusinessRule(UpdateAddressTestCase testCase)
    {
        var addressService = CreateAddressService(Context);
        Context.Users.Add(testCase.User);
        var address = testCase.Address;
        address.UserId =- 1;
        Context.Addresses.Add(testCase.Address);
        await Context.SaveChangesAsync();
        
        Func<Task> act = async () => await addressService.UpdateAddress(testCase.User.Id, testCase.AddressInputDto);

        await act.Should().ThrowAsync<BusinessRuleException>();
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

        await act.Should().ThrowAsync<NotFoundException>();
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

        await act.Should().ThrowAsync<NotFoundException>();
    }
    
    [Theory]
    [ClassData(typeof(UpdateAddressesTestData))]
    public async Task UpdateAddress_WhereCalledWithCorrectData(UpdateAddressTestCase testCase)
    {
        var addressService = CreateAddressService(Context);
        
        Context.Users.Add(testCase.User);
        Context.Addresses.Add(testCase.Address);
        await Context.SaveChangesAsync();
        
        var act = await addressService.UpdateAddress(testCase.User.Id, testCase.AddressInputDto);
        
        act.Should().BeEquivalentTo(testCase.ExpectedResult);
    }
}
