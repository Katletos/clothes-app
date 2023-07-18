using Application.Dtos.Addresses;
using Bogus;
using Domain.Entities;
using Domain.Enums;

namespace UnitTests.AddressService.UpdateAddress;

public class UpdateAddressesTestData : TestDataBase<UpdateAddressTestCase>
{
    protected override IEnumerable<UpdateAddressTestCase> GetTestData()
    {
        var faker = new Faker();
        var userId = faker.Random.Long(1);
        var addressId = faker.Random.Long(1);
        var address = faker.Address.FullAddress();
        yield return new UpdateAddressTestCase()
        {
            Description = "Update address",
            User = new User()
            {
                Id = userId,
                Email = faker.Internet.Email(),
                Password = faker.Internet.Password(),
                Phone = faker.Phone.PhoneNumber(),
                CreatedAt = DateTime.Now,
                UserType = faker.PickRandom<UserType>(),
                FirstName = faker.Name.FirstName(),
                LastName = faker.Name.LastName(),
            },
            Address = new Address() { Id = addressId, UserId = userId, AddressLine = address },
            AddressInputDto = new AddressInputDto() { Id = addressId, AddressLine = address },
            ExpectedResult = new AddressDto() { Id = addressId, UserId = userId, AddressLine = address },
        };
    }
}