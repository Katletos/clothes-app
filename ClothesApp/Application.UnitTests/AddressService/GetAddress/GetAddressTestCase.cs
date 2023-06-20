using Application.Dtos.Addresses;
using Domain.Entities;

namespace UnitTests.AddressService.GetAddress;

public class GetAddressTestCase
{
    public string Description { get; set; }
    
    public User User { get; set; }
    
    public List<AddressDto> ExpectedResult { get; set; }
    
    public List<Address> Addresses { get; set; }
    
    public AddressInputDto AddressInputDto { get; set; }
}