namespace ClothesApp.Models;

public class Address
{
    public long Id { get; set; }

    public long UserId { get; set; }

    public string Address1 { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual User User { get; set; } = null!;
}
