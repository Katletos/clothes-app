using Domain.Enums;

namespace Domain.Entities;
public class User
{
    public long Id { get; set; }
    
    public UserType UserType { get; set; } 

    public string Email { get; set; }

    public string Password { get; set; }

    public string Phone { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<Address> Addresses { get; set; }

    public virtual ICollection<Order> Orders { get; set; }

    public virtual ICollection<Review> Reviews { get; set; }
}
