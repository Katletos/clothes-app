using Bogus;
using Domain.Entities;
using Domain.Enums;

namespace UnitTests;

public class EntitiesGenerator
{
    public static User GenerateUser()
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
}