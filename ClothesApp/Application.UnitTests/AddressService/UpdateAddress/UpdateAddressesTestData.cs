using Application.Dtos.Addresses;
using Domain.Entities;

namespace UnitTests.AddressService.UpdateAddress;

public class UpdateAddressesTestData : TestDataBase<UpdateAddressTestCase>
{
    protected override IEnumerable<UpdateAddressTestCase> GetTestData()
    {
        yield return new UpdateAddressTestCase()
        {
            Description = "Сase of the same ids",
            UserId = 1,
            AddressInputDto = new AddressInputDto() {Id = 1, AddressLine = "Minsk"},
            AddressFromRepository = new Address() {Id = 1, AddressLine = "Homel", UserId = 1},
            ExpectedResult = new AddressDto() {Id = 1, AddressLine = "Minsk", UserId = 1},
        };
        yield return new UpdateAddressTestCase()
        {
            Description = "Сase of the same address",
            UserId = 1,
            AddressInputDto = new AddressInputDto() {Id = 1, AddressLine = "Minsk"},
            AddressFromRepository = new Address() {Id = 1, AddressLine = "Minsk", UserId = 2},
            ExpectedResult = new AddressDto() {Id = 1, AddressLine = "Minsk", UserId = 2},
        };
        yield return new UpdateAddressTestCase()
        {
            Description = "Сase of different addresses",
            UserId = 51,
            AddressInputDto = new AddressInputDto() {Id = 7, AddressLine = "Pinsk"},
            AddressFromRepository = new Address() {Id = 7, AddressLine = "Grodno", UserId = 51},
            ExpectedResult = new AddressDto() {Id = 7, AddressLine = "Pinsk", UserId = 51},
        };
    }
}