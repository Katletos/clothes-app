namespace ClothesApp.Models;

public class Section
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();
}
