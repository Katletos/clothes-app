using Application.Dtos.Addresses;
using Domain.Entities;

namespace UnitTests.AddressService.GetAddress;

public class GetAddressesTestData : TestDataBase<GetAddressTestCase>
{
    protected override IEnumerable<GetAddressTestCase> GetTestData()
    {
        yield return new GetAddressTestCase()
        {
            Description = "Case with one user address",
            UserId = 1,
            Addresses = new List<Address>() 
            {
                new() { Id = 1, UserId = 15, AddressLine = "Minsk", }
            },
            ExpectedResult = new List<AddressDto>() 
            {
                new() { Id = 1, UserId = 15, AddressLine = "Minsk", }
            }
        };
        yield return new GetAddressTestCase()
        {
            Description = "Case of multiple user addresses",
            UserId = 2,
            Addresses = new List<Address>() 
            {
                new() { Id = 42, UserId = 2, AddressLine = "Grodno", },
                new() { Id = 1, UserId = 15, AddressLine = "Minsk", }
            },
            ExpectedResult = new List<AddressDto>()
            {
                new() { Id = 42, UserId = 2, AddressLine = "Grodno", },
                new() { Id = 1, UserId = 15, AddressLine = "Minsk", }
            }
        };
        yield return new GetAddressTestCase()
        {
            Description = "If user have no addresses return empty list",
            UserId = 3,
            Addresses = new List<Address>() {},
            ExpectedResult = new List<AddressDto>() {}
        };
    }
}