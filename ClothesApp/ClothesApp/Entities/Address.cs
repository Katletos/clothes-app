namespace ClothesApp.Entities;

public class Address
{
    public long Id { get; set; }

    public long UserId { get; set; }

    public string AddressLine { get; set; }

    public virtual ICollection<Order> Orders { get; set; }

    public virtual User User { get; set; }
}
