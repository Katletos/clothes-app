using Application.Dtos.Addresses;
using Domain.Entities;

namespace UnitTests.AddressService.AddAddress;

public class AddAddressTestData : TestDataBase<AddAddressTestCase>
{
    protected override IEnumerable<AddAddressTestCase> GetTestData()
    {
        yield return new AddAddressTestCase()
        {
            Description = "Add address for user with id = 1",
            AddAddressDto = new AddAddressDto()
            {
                UserId = 1, AddressLine = "Minsk"
            },
            ExpectedAddressToInsert = new Address()
            {
                Id = 1, UserId = 1, AddressLine = "Minsk"
            },
            ExpectedResult = new AddressDto()
            {
                Id = 1, UserId = 1, AddressLine = "Minsk"
            }, 
        };
        yield return new AddAddressTestCase()
        {
            Description = "Add address for user with id = 2",
            AddAddressDto = new AddAddressDto()
            {
                UserId = 22, AddressLine = "Grodno"
            },
            ExpectedAddressToInsert = new Address()
            {
                Id = 521, UserId = 22, AddressLine = "Grodno"
            },
            ExpectedResult = new AddressDto()
            {
                Id = 521, UserId = 22, AddressLine = "Grodno"
            }, 
        };
    }
}