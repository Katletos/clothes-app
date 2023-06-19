using Application.Dtos.Addresses;
using Bogus;
using Domain.Entities;
using Domain.Enums;
using static UnitTests.EntitiesGenerator;

namespace UnitTests.AddressService.GetAddress;

public class GetAddressesTestData : TestDataBase<GetAddressTestCase>
{
    protected override IEnumerable<GetAddressTestCase> GetTestData()
    {
        var faker = new Faker();
        var id = faker.Random.Long(1);
        var userId = faker.Random.Long(1);
        var addressLine = faker.Address.FullAddress();
        yield return new GetAddressTestCase()
        {
            Description = "Case with one user address",
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
            AddressInputDto = new AddressInputDto()
            {
                 Id = id, 
                 AddressLine = addressLine
            },
            Addresses = new List<Address>() 
            {
                new()
                {
                    Id = id,
                    UserId = userId,
                    AddressLine = addressLine,
                }
            },
            ExpectedResult = new List<AddressDto>() 
            {
                new()
                {
                    Id = id,
                    UserId = userId,
                    AddressLine = addressLine,
                }
            }
        };
        yield return new GetAddressTestCase()
        {
            Description = "If user have no addresses return empty list",
            User = GenerateUser(),
            Addresses = new List<Address>() {},
            ExpectedResult = new List<AddressDto>() {}
        };
    }
}