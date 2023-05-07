namespace ClothesApp.Models;

public class Media
{
    public long Id { get; set; }

    public long ProductId { get; set; }

    public string Url { get; set; } = null!;

    public string FileType { get; set; } = null!;

    public string FileName { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
