namespace Application.Dtos.Addresses;

public struct AddressDto
{
    public long Id { get; set; }

    public long UserId { get; set; }

    public string AddressLine { get; set; }
}