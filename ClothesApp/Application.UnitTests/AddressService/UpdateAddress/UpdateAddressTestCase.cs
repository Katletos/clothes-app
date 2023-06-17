using Application.Dtos.Addresses;
using Domain.Entities;

namespace UnitTests.AddressService.UpdateAddress;

public class UpdateAddressTestCase
{
    public string Description { get; set; }
    
    public long UserId { get; set; }

    public AddressInputDto AddressInputDto { get; set; }

    public Address AddressFromRepository { get; set; }
    
    public AddressDto ExpectedResult { get; set; }
}