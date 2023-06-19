using Application.Dtos.Addresses;
using Domain.Entities;

namespace UnitTests.AddressService.AddAddress;

public class AddAddressTestCase
{
    public string Description { get; set; }
    
    public AddAddressDto AddAddressDto { get; set; }

    public AddressDto ExpectedResult { get; set; }

    public Address ExpectedAddressToInsert { get; set; }
    
    public User User { get; set; }
}