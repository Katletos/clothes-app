using Application.Dtos.Addresses;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Entities;
using UnitTests.AddressService.AddAddress;
using UnitTests.AddressService.GetAddress;
using UnitTests.AddressService.UpdateAddress;

namespace UnitTests.AddressService;

public class AddressServiceTests
{
    private readonly Mock<IAddressRepository> _addressRepositoryMock;

    private readonly Mock<IUserRepository> _userRepositoryMock;

    private readonly Mock<IMapper> _mapperMock;

    public AddressServiceTests()
    {
        _addressRepositoryMock = new Mock<IAddressRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _mapperMock =  new Mock<IMapper>();
    }

    private Application.Services.AddressService CreateAddressService()
    {
        return new Application.Services.AddressService(_addressRepositoryMock.Object, _userRepositoryMock.Object, _mapperMock.Object);
    }

    [Theory]
    [ClassData(typeof(GetAddressesTestData))]
    public async Task GetAddresses_WhereCalledWithCorrectData(GetAddressTestCase testCase)
    {
        var addressService = CreateAddressService();
        var userId = testCase.UserId;
        _userRepositoryMock.Setup(x => x.DoesExist(
            It.Is<long>(l => l == testCase.UserId))).ReturnsAsync(true);
        _addressRepositoryMock.Setup(x => x.FindByCondition(
            a => a.UserId == userId)).ReturnsAsync(testCase.Addresses);
        _mapperMock.Setup(x => x.Map<IList<AddressDto>>(testCase.Addresses))
            .Returns(testCase.ExpectedResult);
        
        var result = await addressService.GetAddresses(testCase.UserId);
        
        result.Should().BeEquivalentTo(testCase.ExpectedResult);
    }

    [Fact]
    public async Task GetAddresses_WhereCalledWithUndiscoveredUserId_ThrowNotFound()
    {
        var addressService = CreateAddressService();
        _userRepositoryMock.Setup(x => x.DoesExist(1)).ReturnsAsync(false);

        Func<Task> act = async () => await addressService.GetAddresses(1);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Theory]
    [ClassData(typeof(AddAddressTestData))]
    public async Task AddAddress_WhereCalledWithCorrectAddAddressDto(AddAddressTestCase testCase)
    {
        var addressService = CreateAddressService();
        Address address = null;
        _userRepositoryMock.Setup(x => x.DoesExist(testCase.AddAddressDto.UserId))
            .ReturnsAsync(true);
        _mapperMock.Setup(x => x.Map<Address>(testCase.AddAddressDto))
            .Returns(testCase.ExpectedAddressToInsert);
        _addressRepositoryMock.Setup(x => x.Insert(It.IsAny<Address>())).
            Callback<Address>((obj) => address = obj);
        _mapperMock.Setup(x => x.Map<AddressDto>(testCase.ExpectedAddressToInsert))
            .Returns(testCase.ExpectedResult);
        
        var act = await addressService.AddAddress(testCase.AddAddressDto);

        address.Should().BeEquivalentTo(testCase.ExpectedAddressToInsert);
        act.Should().BeEquivalentTo(testCase.ExpectedResult);
    }

    
    [Theory]
    [InlineData(1, 1)]
    [InlineData(2, 2)]
    [InlineData(1, 2)]
    public async Task UpdateAddress_WhereCalledWithANonRelativeUserAddress_ThrowsBusinessRule(long addressId, long userId)
    {
        var addressService = CreateAddressService();
        var addressInputDto = new AddressInputDto();
        _addressRepositoryMock.Setup(x => x.DoesAddressBelongToUser(
                It.Is<long>(l => l == addressId),
                It.Is<long>(l => l == userId)))
            .ReturnsAsync(false);
            
        Func<Task> act = async () => await addressService.UpdateAddress(1, addressInputDto);

        await act.Should().ThrowAsync<NotFoundException>();
    }
    
    [Theory]
    [InlineData(1)]
    [InlineData(132)]
    [InlineData(112315151)]
    public async Task Update_WhereCalledWithUndiscoveredUserId_ThrowsNotFound(long userId)
    {
        var addressService = CreateAddressService();
        var addressInputDto = new AddressInputDto(); 
        _userRepositoryMock.Setup(x => x.DoesExist(It.Is<long>(l => l == userId))).ReturnsAsync(false);

        Func<Task> act = async () => await addressService.UpdateAddress(1, addressInputDto);

        await act.Should().ThrowAsync<NotFoundException>();
    }
    
    [Theory]
    [InlineData(1, 1)]
    [InlineData(2, 2)]
    [InlineData(1, 2)]
    public async Task UpdateAddress_WhereCalledWithUndiscoveredAddressId_ThrowsNotFound(long userId, long addressId)
    {
        var addressService = CreateAddressService();
        var addressInputDto = new AddressInputDto(); 
        _userRepositoryMock.Setup(x => x.DoesExist(It.Is<long>(l => l == userId))).ReturnsAsync(true);
        _addressRepositoryMock.Setup(x => x.DoesAddressBelongToUser(addressId, userId))
            .ReturnsAsync(true);
        _addressRepositoryMock.Setup(x => x.DoesExist(It.Is<long>(l => l == addressId))).ReturnsAsync(false);

        Func<Task> act = async () => await addressService.UpdateAddress(1, addressInputDto);

        await act.Should().ThrowAsync<NotFoundException>();
    }
    
    [Theory]
    [ClassData(typeof(UpdateAddressesTestData))]
    public async Task UpdateAddress_WhereCalledWithCorrectData(UpdateAddressTestCase testCase)
    {
        var addressService = CreateAddressService();
        Address address = null;
        _userRepositoryMock.Setup(x => x.DoesExist(It.Is<long>(l => l == testCase.UserId)))
            .ReturnsAsync(true);
        _addressRepositoryMock.Setup(x => x.DoesExist(It.Is<long>(l => l == testCase.AddressInputDto.Id)))
            .ReturnsAsync(true);
        _addressRepositoryMock.Setup(x => x.DoesAddressBelongToUser(
                It.Is<long>(l => l == testCase.AddressInputDto.Id), 
                It.Is<long>(l => l == testCase.UserId)))
            .ReturnsAsync(true);
        _addressRepositoryMock.Setup(x => x.GetById(
                It.Is<long>(l => l == testCase.AddressInputDto.Id)))
            .ReturnsAsync(testCase.AddressFromRepository);
        _mapperMock.Setup(x => x.Map<Address, AddressDto>(testCase.AddressFromRepository))
            .Returns(testCase.ExpectedResult);
        _addressRepositoryMock.Setup(x => x.Update(It.IsAny<Address>())).
            Callback<Address>((obj) => address = obj);
        
        var act = await addressService.UpdateAddress(testCase.UserId, testCase.AddressInputDto);

        address.Should().BeEquivalentTo(testCase.AddressFromRepository);
        act.Should().BeEquivalentTo(testCase.ExpectedResult);
    }
}
