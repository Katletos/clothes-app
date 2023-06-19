using Application.Dtos.Addresses;
using Bogus;
using Domain.Entities;
using Domain.Enums;

namespace UnitTests.AddressService.AddAddress;

public class AddAddressTestData : TestDataBase<AddAddressTestCase>
{
    protected override IEnumerable<AddAddressTestCase> GetTestData()
    {
        var faker = new Faker();
        var userId = faker.Random.Long(1);
        var addressLine = faker.Address.FullAddress();
        yield return new AddAddressTestCase()
        {
            Description = "Add address",
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
            AddAddressDto = new AddAddressDto()
            {
                UserId = userId,
                AddressLine = addressLine,
            },
        };
    }
}