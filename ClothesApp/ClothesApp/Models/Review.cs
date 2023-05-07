namespace ClothesApp.Models;

public class Review
{
    public long Id { get; set; }

    public long ProductId { get; set; }

    public long UserId { get; set; }

    public short Rating { get; set; }

    public string Title { get; set; } = null!;

    public string Comment { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
